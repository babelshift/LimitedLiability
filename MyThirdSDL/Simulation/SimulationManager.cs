using System;
using System.Collections.Generic;
using System.Linq;
using SharpDL;
using SharpDL.Graphics;
using MyThirdSDL.Agents;
using MyThirdSDL.Content;

namespace MyThirdSDL.Simulation
{
	public enum SimulationState
	{
		NotStarted,
		Running,
		Paused
	}

	public class SimulationManager
	{
		#region Members

		public static readonly int SimulationTimeToWorldTimeMultiplier = 540;
		private readonly IEnumerable<ThoughtMetadata> thoughtPool;
		private readonly List<Agent> trackedAgents = new List<Agent>();
		private DateTime startingWorldDateTime;
		private readonly Random random = new Random();
		private TiledMap currentMap;
		private SimulationState state;

		#endregion Members

		#region Properties

		/// <summary>
		/// Gets and sets the simulation state. This will only change the state if the new value is not equal to the current value.
		/// </summary>
		public SimulationState State
		{
			get { return state; }
			set
			{
				if (state != value)
				{
					state = value;
					if (state == SimulationState.Paused)
						SimulationTimeAtPause = SimulationTime;
					else if (state == SimulationState.Running)
						SimulationTime = SimulationTimeAtPause;
				}
			}
		}

		/// <summary>
		/// The time at which the last pause occurred. This is the time that will be used when the unpause occurs.
		/// </summary>
		private TimeSpan SimulationTimeAtPause { get; set; }

		public DateTime WorldDateTime { get { return startingWorldDateTime.Add(WorldTimePassed); } }

		private static TimeSpan WorldTimePassed
		{
			get
			{
				double worldMilliseconds = SimulationTimeToWorldTimeMultiplier * SimulationTime.TotalMilliseconds;
				TimeSpan worldTimePassed = TimeSpan.FromMilliseconds(worldMilliseconds);
				return worldTimePassed;
			}
		}

		public static TimeSpan SimulationTime { get; private set; }

		/// <summary>
		/// Returns an enumerable of all tracked agents in the simulation.
		/// </summary>
		/// <value>The tracked agents.</value>
		public IEnumerable<Agent> TrackedAgents { get { return trackedAgents; } }

		public IEnumerable<Employee> TrackedEmployees
		{
			get { return GetTrackedAgentsByType<Employee>(); }
		}

		public IEnumerable<Equipment> TrackedEquipment
		{
			get { return GetTrackedAgentsByType<Equipment>(); }
		}

		#endregion Properties

		#region Public Simulation Events

		public event EventHandler<ThoughtEventArgs> HadThought;
		public event EventHandler<EventArgs> EmployeeThirstSatisfied;
		public event EventHandler<EventArgs> EmployeeHungerSatisfied;

		#endregion Public Simulation Events

		#region Private Simulation Event Handlers

		#endregion Private Simulation Event Handlers

		public SimulationManager(DateTime startingWorldDateTime, IEnumerable<ThoughtMetadata> thoughtPool)
		{
			this.startingWorldDateTime = startingWorldDateTime;
			this.thoughtPool = thoughtPool;
		}

		/// <summary>
		/// Updates the simulation by setting the simulation time and updating all tracked agents.
		/// </summary>
		/// <param name="gameTime">Game time.</param>
		public void Update(GameTime gameTime)
		{
			// only update if the simulation isn't paused
			if (state == SimulationState.Paused)
				return;

			SimulationTime += gameTime.ElapsedGameTime;

			foreach (var trackedAgent in trackedAgents)
			{
				trackedAgent.Update(gameTime);

				var employee = trackedAgent as Employee;
				if (employee != null)
				{
					// TODO: delay updating this until 1-2 seconds has passed? we don't really need to update this often
					foreach (var unsatisfiedThought in employee.UnsatisfiedThoughts)
						TakeActionBasedOnThought(employee, unsatisfiedThought.Type);

					// update the employee's age based on the updated world date time
					employee.UpdateAge(WorldDateTime);
				}
			}
		}

		#region Employee Events

		private void HandleHadThought(object sender, ThoughtEventArgs e)
		{
			Thought thought = GenerateThought(e.Type);
			Employee employee = sender as Employee;
			if (employee != null)
			{
				employee.AddUnsatisfiedThought(thought);
				TakeActionBasedOnThought(employee, thought.Type);
				if (HadThought != null)
					HadThought(sender, e);
			}
		}

		private void TakeActionBasedOnThought(Employee employee, ThoughtType thoughtType)
		{
			switch (thoughtType)
			{
				case ThoughtType.Hungry:
					WalkMobileAgentToClosest<SnackMachine>(employee);
					break;
				case ThoughtType.Thirsty:
					WalkMobileAgentToClosest<SodaMachine>(employee);
					break;
				case ThoughtType.NeedsDeskAssignment:
					WalkMobileAgentToClosest<OfficeDesk>(employee);
					break;
				case ThoughtType.IsIdle:
					WalkEmployeeToAssignedOfficeDesk(employee);
					break;
			}
		}

		#endregion Employee Events

		#region Agent Tracking

		/// <summary>
		/// Determines whether the passed agent is already tracked by the simulation. The passed type T is used as a key.
		/// </summary>
		/// <returns><c>true</c> if this instance is agent already tracked the specified agent; otherwise, <c>false</c>.</returns>
		/// <param name="agent">Agent.</param>
		/// <typeparam name="T">The 1st type parameter.</typeparam>
		private bool IsAgentAlreadyTracked<T>(T agent)
			where T : Agent
		{
			IEnumerable<T> agentsForType = GetTrackedAgentsByType<T>();
			return agentsForType.Any(a => a.ID == agent.ID);
		}

		/// <summary>
		/// Adds the passed collection of agents to the simulation.
		/// </summary>
		/// <param name="agents">Agents.</param>
		public void AddAgents<T>(IEnumerable<T> agents)
			where T : Agent
		{
			foreach (var agent in agents)
				AddAgent(agent);
		}

		/// <summary>
		/// Adds the passed agent to the simulation (agent will be updated in the game loop). Will also subscribe to all agent events and bubble them accordingly.
		/// </summary>
		/// <param name="agent">Agent.</param>
		public void AddAgent<T>(T agent)
			where T : Agent
		{
			if (IsAgentAlreadyTracked(agent))
				return;

			agent.Activate();

			if (agent is Employee)
			{
				var employee = agent as Employee;

				employee.HadThought += HandleHadThought;
				employee.ThoughtSatisfied += HandleThoughtSatisfied;

				StartTrackingAgent(employee);
			}
			else
				StartTrackingAgent(agent);
		}

		private void HandleThoughtSatisfied(object sender, ThoughtEventArgs e)
		{
			if (e.Type == ThoughtType.Hungry)
				EventHelper.FireEvent(EmployeeHungerSatisfied, sender, e);
			else if (e.Type == ThoughtType.Thirsty)
				EventHelper.FireEvent(EmployeeThirstSatisfied, sender, e);
		}

		/// <summary>
		/// Starts tracking the agent in the simulation. The type T is used as a key.
		/// </summary>
		/// <param name="agent">Agent.</param>
		/// <typeparam name="T">The 1st type parameter.</typeparam>
		private void StartTrackingAgent(Agent agent)
		{
			if (!trackedAgents.Any(a => a.ID == agent.ID))
				trackedAgents.Add(agent);
		}

		/// <summary>
		/// Stops tracking the agent identified by the passed Guid ID. The type T is used as a key.
		/// </summary>
		/// <param name="agentId">Agent identifier.</param>
		/// <typeparam name="T">The 1st type parameter.</typeparam>
		private void StopTrackingAgent(Guid agentId)
		{
			Agent agentToRemove = trackedAgents.FirstOrDefault(ta => ta.ID == agentId);
			if (agentToRemove != null)
			{
				trackedAgents.Remove(agentToRemove);
				agentToRemove.Dispose();
			}
		}

		/// <summary>
		/// Removes an agent identified by the passed Guid from the simulation. Will also unsubscribe the simulation from any events the agent fires.
		/// </summary>
		/// <param name="agentId">Agent identifier.</param>
		public void RemoveAgent<T>(Guid agentId)
			where T : Agent
		{
			var agent = GetTrackedAgent<T>(agentId);
			if (agent == null)
				return;
			
			agent.Deactivate();

			var employee = agent as Employee;
			if (employee != null)
			{
				employee.HadThought -= HandleHadThought;
				employee.ThoughtSatisfied -= HandleThoughtSatisfied;
			}

			StopTrackingAgent(agentId);
		}

		/// <summary>
		/// Returns the tracked agent identified by the passed Guid ID. The type T is used as a key. If the passed Guid does not match to any tracked agents,
		/// null is returned.
		/// </summary>
		/// <returns>The tracked agent.</returns>
		/// <param name="agentId">Agent identifier.</param>
		/// <typeparam name="T">The 1st type parameter.</typeparam>
		public T GetTrackedAgent<T>(Guid agentId)
			where T : Agent
		{
			return (T)trackedAgents.FirstOrDefault(ta => ta.ID == agentId);
		}

		private IEnumerable<T> GetTrackedAgentsByType<T>()
			where T : Agent
		{
			// return all agents that are exactly of the generic T or a sub class of the generic T
			IEnumerable<Agent> agentsByType = trackedAgents
				.Where(ta => 
					ta is T 
					|| ta.GetType().IsSubclassOf(typeof(T)));

			return agentsByType.Cast<T>();
		}

		public void PromoteEmployee(Guid employeeId)
		{
			Employee employee = GetTrackedAgent<Employee>(employeeId);
			employee.Promote();
		}

		public void DemoteEmployee(Guid employeeId)
		{
			Employee employee = GetTrackedAgent<Employee>(employeeId);
			employee.Demote();
		}

		#endregion Agent Tracking

		#region Employee Triggers

		/// <summary>
		/// Attempts to walk the employee to its assigned office desk. This will queue up an intention of "Go To Desk" for the employee.
		/// </summary>
		/// <param name="employee">Employee.</param>
		private void WalkEmployeeToAssignedOfficeDesk(Employee employee)
		{
			AddIntentionToAgent(employee, employee.AssignedOfficeDesk, IntentionType.GoToDesk);
		}

		/// <summary>
		/// Walks the passed mobile agent to the closest agent of type T.
		/// </summary>
		/// <param name="agent">Agent.</param>
		/// <typeparam name="T">The 1st type parameter.</typeparam>
		public void WalkMobileAgentToClosest<T>(MobileAgent mobileAgent)
			where T : Agent
		{
			IntentionType intentionType = IntentionType.Unknown;

			if (typeof(T) == typeof(SodaMachine))
				intentionType = IntentionType.BuyDrink;
			else if (typeof(T) == typeof(SnackMachine))
				intentionType = IntentionType.BuySnack;
			else if (typeof(T) == typeof(OfficeDesk))
				intentionType = IntentionType.GoToDesk;

			// if we don't already intend to perform that intention, proceed
			if (mobileAgent.IsAlreadyIntention(intentionType))
				return;
			
			var agentsToCheck = GetTrackedAgentsByType<T>();

			// if there are agents by that type to head towards, proceed
			if (agentsToCheck.Count() == 0)
				return;
			
			// find the closest agent by the type T to the employee and set the employee on his way towards that agent if any exists
			var closestAgent = GetClosestAgentByType(mobileAgent, agentsToCheck);

			// if there is an actual closest agent in the simulation, proceed
			if (closestAgent == null)
				return;
			
			// if this is a triggerable, subscribe to the trigger now that we intend to go to it
			//						if (closestAgent is ITriggerable)
			//						{
			//							var triggerable = closestAgent as ITriggerable;
			//							if (closestAgent is SodaMachine)
			//								triggerable.Trigger.AddSubscriptionToActionByType(mobileAgent, SubscriptionType.Once, ActionType.DispenseDrink);
			//							if (closestAgent is SnackMachine)
			//								triggerable.Trigger.AddSubscriptionToActionByType(mobileAgent, SubscriptionType.Once, ActionType.DispenseFood);
			//						}

			// we only want to add a "go to office desk" intention if the desk is not assigned to someone
			if (closestAgent is OfficeDesk)
			{
				var closestOfficeDesk = closestAgent as OfficeDesk;
				if (!closestOfficeDesk.IsAssignedToAnEmployee)
					AddIntentionToAgent(mobileAgent, closestAgent, intentionType);
			}
			else
				AddIntentionToAgent(mobileAgent, closestAgent, intentionType);
		}

		/// <summary>
		/// Adds and intention based on the passed intention type to the from agent based on the passed to agent. Basically, the "fromAgent"
		/// will perform whatever intention is indicated by the "intentionType" on the "toAgent", such as walking to a snack machine to drink.
		/// </summary>
		/// <param name="fromAgent">From agent.</param>
		/// <param name="toAgent">To agent.</param>
		/// <param name="intentionType">Intention type.</param>
		private void AddIntentionToAgent(MobileAgent fromAgent, Agent toAgent, IntentionType intentionType)
		{
			Agent walkFromAgent;

			Intention finalIntention = fromAgent.FinalIntention;

			// if we have a final intention currently queued up, then we want to calculate the path from that position
			if (finalIntention != null)
				walkFromAgent = finalIntention.WalkToAgent;
			// otherwise, we want to calculate the path from our current position
			else
				walkFromAgent = fromAgent;

			var bestPathToClosestAgent = GetBestPathToAgent(walkFromAgent, toAgent);

			fromAgent.AddIntention(new Intention(toAgent, bestPathToClosestAgent, intentionType));
		}

		#endregion Employee Triggers

		#region Path Finding

		/// <summary>
		/// Finds the nodes at the passed world grid indices and returns a queue of map objects to travel along in order to get from start
		/// to end.
		/// </summary>
		/// <param name="startWorldPosition"></param>
		/// <param name="endWorldPosition"></param>
		/// <returns></returns>
		private Queue<PathNode> FindBestPath(Vector startWorldPosition, Vector endWorldPosition)
		{
			PathNode start = currentMap.GetPathNodeAtWorldPosition(startWorldPosition);
			PathNode end = currentMap.GetPathNodeAtWorldPosition(endWorldPosition);
			Path<PathNode> bestPath = FindPath(start, end);//, ExactDistance, ManhattanDistance);
			IEnumerable<PathNode> bestPathReversed = bestPath.Reverse();
			Queue<PathNode> result = new Queue<PathNode>();
			foreach (var bestPathNode in bestPathReversed)
				result.Enqueue(bestPathNode);
			return result;
		}

		/// <summary>
		/// An implementation of the A* path finding algorithm. Finds the best path betwen the passed start and end nodes while utilizing
		/// the passed exact distance function and estimated heuristic distance function.
		/// </summary>
		/// <typeparam name="TNode"></typeparam>
		/// <param name="start"></param>
		/// <param name="destination"></param>
		/// <returns></returns>
		private Path<TNode> FindPath<TNode>(
			TNode start,							// starting node
			TNode destination)//,					// destination node
			//Func<Node, Node, double> distance,	// takes two nodes and calculates a distance cost between them
			//Func<Node, Node, double> estimate)		// takes a node and calculates an estimated distance between current node
			where TNode : INode, IHasNeighbors<TNode>
		{
			var closed = new HashSet<TNode>();
			var queue = new PriorityQueue<double, Path<TNode>>();

			queue.Enqueue(0, new Path<TNode>(start));

			while (!queue.IsEmpty)
			{
				var path = queue.Dequeue();

				if (closed.Contains(path.LastStep))
					continue;

				if (path.LastStep.Equals(destination))
					return path;

				closed.Add(path.LastStep);

				foreach (TNode n in path.LastStep.Neighbors)
				{
					double d = ExactDistance(path.LastStep, n);
					var newPath = path.AddStep(n, d);
					queue.Enqueue(newPath.TotalCost + ManhattanDistanceEstimate(n, destination), newPath);
				}
			}

			return null;
		}

		/// <summary>
		/// Using a list of agents, this method returns the agent which is located closest (by manhatten distance) to the passed mobile agent.
		/// </summary>
		/// <returns>The closest agent by type.</returns>
		/// <param name="mobileAgent">Mobile agent.</param>
		/// <param name="agentsToCheck">Agents to check.</param>
		/// <typeparam name="T">The 1st type parameter.</typeparam>
		private T GetClosestAgentByType<T>(MobileAgent mobileAgent, IEnumerable<T> agentsToCheck)
			where T : Agent
		{
			if (mobileAgent == null)
				throw new ArgumentNullException("mobileAgent");

			if (agentsToCheck.Count() > 0)
			{
				// calculate the closest snack machine's manhatten distance
				int minimumDistance = Int32.MaxValue;
				T closestAgent = null;

				foreach (var agentToCheck in agentsToCheck)
				{
					// we want to remove assigned office desks from the list of closest agents because they should be considered taken, so skip over them
					if (agentToCheck is OfficeDesk)
					{
						var officeDesk = agentToCheck as OfficeDesk;
						if (officeDesk.IsAssignedToAnEmployee)
							continue;
					}

					// TODO: rethink this? we are looking up the best path in an inefficient way
					var bestPath = GetBestPathToAgent(mobileAgent, agentToCheck);

					if (bestPath.Count < minimumDistance)
					{
						minimumDistance = bestPath.Count;
						closestAgent = agentToCheck;
					}
				}

				return closestAgent;
			}
			
			return null;
		}

		/// <summary>
		/// Gets the best path from the passed mobile agent to the passed agent using A* path finding.
		/// </summary>
		/// <returns>The best path to agent.</returns>
		/// <param name="mobileAgent">Mobile agent.</param>
		/// <param name="agent">Agent.</param>
		/// <typeparam name="T">The 1st type parameter.</typeparam>
		private Queue<PathNode> GetBestPathToAgent<T>(Agent mobileAgent, T agent)
			where T : Agent
		{
			// find the best path from the mobile agent's center to the target agent's center
			Queue<PathNode> bestPath = FindBestPath(
				                           new Vector(mobileAgent.CollisionBox.Center.X, mobileAgent.CollisionBox.Center.Y),
				                           new Vector(agent.CollisionBox.Center.X, agent.CollisionBox.Center.Y));

			return bestPath;
		}

		/// <summary>
		/// The exact distance between two nodes in this game is a single node (1). By default, node links do not have costs associated with them.
		/// </summary>
		/// <typeparam name="TNode"></typeparam>
		/// <param name="node1"></param>
		/// <param name="node2"></param>
		/// <returns></returns>
		private static double ExactDistance<TNode>(TNode node1, TNode node2)
			where TNode : INode
		{
			return 1.0;
		}

		/// <summary>
		/// The manhattan distance between two nodes is the distance traveled on a taxicab-like grid where diagonal movement is not allowed.
		/// </summary>
		/// <typeparam name="TNode"></typeparam>
		/// <param name="node1"></param>
		/// <param name="node2"></param>
		/// <returns></returns>
		private double ManhattanDistanceEstimate<TNode>(TNode node1, TNode node2)
			where TNode : INode
		{
			return Math.Abs(node1.WorldPosition.X - node2.WorldPosition.X) + Math.Abs(node1.WorldPosition.Y - node2.WorldPosition.Y);
		}

		#endregion Path Finding

		private Thought GenerateThought(ThoughtType type)
		{
			ThoughtMetadata[] thoughtMatches = thoughtPool.Where(t => t.Type == type).ToArray();
			int randomIndex = random.Next(0, thoughtMatches.Count() - 1);
			ThoughtMetadata thought = thoughtMatches[randomIndex];
			return new Thought(thought.Idea, thought.Type, WorldDateTime, SimulationTime);
		}

		public void SetCurrentMap(TiledMap tiledMap)
		{
			var mapEquipmentOccupents = tiledMap.GetEquipmentOccupants();
			
			foreach (var mapEquipmentOccupent in mapEquipmentOccupents)
			{
				if (mapEquipmentOccupent is SnackMachine)
					AddAgent(mapEquipmentOccupent as SnackMachine);
				else if (mapEquipmentOccupent is SodaMachine)
					AddAgent(mapEquipmentOccupent as SodaMachine);
				else if (mapEquipmentOccupent is WaterFountain)
					AddAgent(mapEquipmentOccupent as WaterFountain);
				else if (mapEquipmentOccupent is OfficeDesk)
					AddAgent(mapEquipmentOccupent as OfficeDesk);
			}

			currentMap = tiledMap;
		}
	}
}