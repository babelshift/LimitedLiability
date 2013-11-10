using MyThirdSDL.Descriptors;
using SharpDL;
using SharpDL.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyThirdSDL
{
	public class SimulationManager
	{
		private List<Agent> agents = new List<Agent>();

		public string SimulationTimeDisplay
		{
			get
			{
				return String.Format("{0} minutes, {1} seconds, {2} milliseconds",
					SimulationTime.Minutes.ToString(), SimulationTime.Seconds.ToString(), SimulationTime.Milliseconds.ToString());
			}
		}

		public TimeSpan SimulationTime { get; private set; }

		public TiledMap CurrentMap { get; set; }

		#region Public Simulation Events

		public event EventHandler<EventArgs> EmployeeIsSleepy;
		public event EventHandler<EventArgs> EmployeeIsUnhealthy;
		public event EventHandler<EventArgs> EmployeeIsDirty;
		public event EventHandler<EventArgs> EmployeeIsHungry;
		public event EventHandler<EventArgs> EmployeeIsThirsty;
		public event EventHandler<EventArgs> EmployeeIsUnhappy;
		public event EventHandler<EventArgs> EmployeeNeedsOfficeDesk;

		public event EventHandler<EventArgs> EmployeeThirstSatisfied;
		public event EventHandler<EventArgs> EmployeeHungerSatisfied;

		#endregion

		#region Private Simulation Event Handlers

		#endregion

		public SimulationManager()
		{
		}

		/// <summary>
		/// Updates the simulation by setting the simulation time and updating all tracked agents.
		/// </summary>
		/// <param name="gameTime">Game time.</param>
		public void Update(GameTime gameTime)
		{
			SimulationTime = gameTime.TotalGameTime;

			foreach (var agent in agents)
			{
				agent.Update(gameTime);

//				if (agent is Employee)
//				{
//					var mobileAgent = agent as Employee;
//
//					// if agent is walking towards agent
//					// if agent has reached its walking destination
//					if (mobileAgent.IsWalkingTowardsAgent && mobileAgent.IsAtFinalDestination)
//					{
//						var walkingTowardsAgent = mobileAgent.WalkingTowardsAgent;
//						// if soda machine, drink
//						if (walkingTowardsAgent is SodaMachine)
//						{
//							var sodaMachine = walkingTowardsAgent as SodaMachine;
//							mobileAgent.Drink(sodaMachine.ThirstEffectiveness);
//							mobileAgent.ResetWalkingTowardsAgent();
//						}
//					}
//				}
			}
		}

		#region Employee Events

		private void HandleNeedsOfficeDesk(object sender, EventArgs e)
		{
			var employee = GetEmployeeFromEventSender(sender);
			EventHelper.FireEvent(EmployeeNeedsOfficeDesk, sender, e);
			WalkMobileAgentToClosest<OfficeDesk>(employee);
		}

		private void HandleIsUnhappy(object sender, EventArgs e)
		{
			
		}

		private void HandleIsUnhealthy(object sender, EventArgs e)
		{
			
		}

		private void HandleIsThirsty(object sender, EventArgs e)
		{
			var employee = GetEmployeeFromEventSender(sender);
			EventHelper.FireEvent(EmployeeIsThirsty, sender, e);

			if(!employee.IsWalkingTowardsAgent)
				WalkMobileAgentToClosest<SodaMachine>(employee);
		}

		private void HandleIsHungry(object sender, EventArgs e)
		{
			var employee = GetEmployeeFromEventSender(sender);
			EventHelper.FireEvent(EmployeeIsHungry, sender, e);

			if(!employee.IsWalkingTowardsAgent)
				WalkMobileAgentToClosest<SnackMachine>(employee);
		}

		private void HandleIsDirty(object sender, EventArgs e)
		{
			
		}

		private void HandleIsSleepy(object sender, EventArgs e)
		{

		}

		private Employee GetEmployeeFromEventSender(object sender)
		{
			var employee = sender as Employee;
			if (employee == null)
				throw new ArgumentException("HandleEmployee handlers can only work with Employee objects!");
			return employee;
		}

		#endregion

		/// <summary>
		/// Adds the passed agent to the simulation (agent will be updated in the game loop). Will also subscribe to all agent events and bubble them accordingly.
		/// </summary>
		/// <param name="agent">Agent.</param>
		public void AddAgent(Agent agent)
		{
			if (!agents.Any(a => a.ID == agent.ID))
			{
				agent.Activate();

				if (agent is Employee)
				{
					var employee = agent as Employee;

					employee.IsSleepy += HandleIsSleepy;
					employee.IsDirty += HandleIsDirty;
					employee.IsHungry += HandleIsHungry;
					employee.IsThirsty += HandleIsThirsty;
					employee.IsUnhealthy += HandleIsUnhealthy;
					employee.IsUnhappy += HandleIsUnhappy;
					employee.NeedsOfficeDesk += HandleNeedsOfficeDesk;

					employee.ThirstSatisfied += HandleThirstSatisfied;
					employee.HungerSatisfied += HandleHungerSatisfied;

					agents.Add(employee);
				}
				else
					agents.Add(agent);
			}
		}

		private void HandleHungerSatisfied (object sender, EventArgs e)
		{
			EventHelper.FireEvent(EmployeeHungerSatisfied, sender, e);
		}

		private void HandleThirstSatisfied (object sender, EventArgs e)
		{
			EventHelper.FireEvent(EmployeeThirstSatisfied, sender, e);
		}

		/// <summary>
		/// Removes an agent identified by the passed Guid from the simulation. Will also unsubscribe the simulation from any events the agent fires.
		/// </summary>
		/// <param name="agentId">Agent identifier.</param>
		public void RemoveAgent(Guid agentId)
		{
			var agent = agents.FirstOrDefault(a => a.ID == agentId);
			if (agent != null)
			{
				agent.Deactivate();

				if (agent is Employee)
				{
					var employee = agent as Employee;

					employee.IsSleepy -= EmployeeIsSleepy;
					employee.IsDirty -= EmployeeIsDirty;
					employee.IsHungry -= EmployeeIsHungry;
					employee.IsThirsty -= EmployeeIsThirsty;
					employee.IsUnhealthy -= EmployeeIsUnhealthy;
					employee.IsUnhappy -= EmployeeIsUnhappy;
					employee.NeedsOfficeDesk -= EmployeeNeedsOfficeDesk;

					employee.ThirstSatisfied -= HandleThirstSatisfied;
					employee.HungerSatisfied -= HandleHungerSatisfied;

					agents.Remove(employee);
				}
				else
					agents.Remove(agent);
			}
		}

		private IEnumerable<T> GetAgentsInSimulationByType<T>()
			where T : Agent
		{
			return agents.Where(a => a.GetType() == typeof(T)).Cast<T>();
		}

		/// <summary>
		/// Walks the passed mobile agent to the closest agent of type T.
		/// </summary>
		/// <param name="agent">Agent.</param>
		/// <typeparam name="T">The 1st type parameter.</typeparam>
		public void WalkMobileAgentToClosest<T>(MobileAgent mobileAgent)
			where T : Agent
		{
			var agentsToCheck = GetAgentsInSimulationByType<T>();

			if (agentsToCheck.Count() > 0)
			{
				// find the best path to the closest soda machine to the employee and set the employee on his way towards that soda machine
				var closestAgent = GetClosestAgentByType<T>(mobileAgent, agentsToCheck);

				if (closestAgent is ITriggerable)
				{
					var triggerable = closestAgent as ITriggerable;
					if(closestAgent is SodaMachine)
						triggerable.Trigger.AddSubscriberToActionByType(mobileAgent, ActionType.DispenseDrink);
					if(closestAgent is SnackMachine)
						triggerable.Trigger.AddSubscriberToActionByType(mobileAgent, ActionType.DispenseFood);
				}

				if (closestAgent != null)
				{
					var bestPathToClosestAgent = GetBestPathToAgent(mobileAgent, closestAgent);
					mobileAgent.AddIntent(new Intent(closestAgent, bestPathToClosestAgent));
				}
			}
		}

		#region Path Finding

		/// <summary>
		/// Finds the nodes at the passed world grid indices and returns a queue of map objects to travel along in order to get from start
		/// to end.
		/// </summary>
		/// <param name="startWorldGridIndex"></param>
		/// <param name="endWorldGridIndex"></param>
		/// <returns></returns>
		private Queue<MapObject> FindBestPath(Vector startWorldGridIndex, Vector endWorldGridIndex)
		{
			MapObject start = CurrentMap.GetPathNodeAtWorldGridIndex(startWorldGridIndex);
			MapObject end = CurrentMap.GetPathNodeAtWorldGridIndex(endWorldGridIndex);
			Path<MapObject> bestPath = FindPath<MapObject>(start, end, ExactDistance, ManhattanDistance);
			IEnumerable<MapObject> bestPathReversed = bestPath.Reverse();
			Queue<MapObject> result = new Queue<MapObject>();
			foreach (var bestPathNode in bestPathReversed)
				result.Enqueue(bestPathNode);
			return result;
		}

		/// <summary>
		/// An implementation of the A* path finding algorithm. Finds the best path betwen the passed start and end nodes while utilizing
		/// the passed exact distance function and estimated heuristic distance function.
		/// </summary>
		/// <typeparam name="Node"></typeparam>
		/// <param name="start"></param>
		/// <param name="destination"></param>
		/// <param name="distance"></param>
		/// <param name="estimate"></param>
		/// <returns></returns>
		private Path<Node> FindPath<Node>(
			Node start,							// starting node
			Node destination,					// destination node
			Func<Node, Node, double> distance,	// takes two nodes and calculates a distance cost between them
			Func<Node, Node, double> estimate)		// takes a node and calculates an estimated distance between current node
			where Node : IHasNeighbors<Node>
		{
			var closed = new HashSet<Node>();
			var queue = new PriorityQueue<double, Path<Node>>();

			queue.Enqueue(0, new Path<Node>(start));

			while (!queue.IsEmpty)
			{
				var path = queue.Dequeue();

				if (closed.Contains(path.LastStep))
					continue;

				if (path.LastStep.Equals(destination))
					return path;

				closed.Add(path.LastStep);

				foreach (Node n in path.LastStep.Neighbors)
				{
					double d = distance(path.LastStep, n);
					var newPath = path.AddStep(n, d);
					queue.Enqueue(newPath.TotalCost + estimate(n, destination), newPath);
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
			var employeeWorldIndex = mobileAgent.WorldGridIndex;
			var employeeOnPathNode = CurrentMap.GetPathNodeAtWorldGridIndex(employeeWorldIndex);

			if (agentsToCheck.Count() > 0)
			{
				// calculate the closest snack machine's manhatten distance
				double minimumManhattenDistance = Int32.MaxValue;
				T closestAgent = null;

				foreach (var agentToCheck in agentsToCheck)
				{
					var agentToFindWorldIndex = agentToCheck.WorldGridIndex;
					var agentOnPathNode = CurrentMap.GetPathNodeAtWorldGridIndex(agentToFindWorldIndex);
					double manhattenDistance = ManhattanDistance(employeeOnPathNode, agentOnPathNode);
					if (manhattenDistance < minimumManhattenDistance)
					{
						minimumManhattenDistance = manhattenDistance;
						closestAgent = agentToCheck;
					}
				}

				return closestAgent;
			}
			else
				return null;
		}

		/// <summary>
		/// Gets the best path from the passed mobile agent to the passed agent using A* path finding.
		/// </summary>
		/// <returns>The best path to agent.</returns>
		/// <param name="mobileAgent">Mobile agent.</param>
		/// <param name="agent">Agent.</param>
		/// <typeparam name="T">The 1st type parameter.</typeparam>
		private Queue<MapObject> GetBestPathToAgent<T>(MobileAgent mobileAgent, T agent)
			where T : Agent
		{
			// tell the agent to path to the closest soda machine (or random if a tie)
			Queue<MapObject> bestPath = FindBestPath(mobileAgent.WorldGridIndex, agent.WorldGridIndex);
			return bestPath;
		}

		/// <summary>
		/// The exact distance between two nodes in this game is a single node (1).
		/// </summary>
		/// <typeparam name="Node"></typeparam>
		/// <param name="node1"></param>
		/// <param name="node2"></param>
		/// <returns></returns>
		private double ExactDistance<Node>(Node node1, Node node2)
			where Node : INode
		{
			return 1.0;
		}

		/// <summary>
		/// The manhattan distance between two nodes is the distance traveled on a taxicab-like grid where diagonal movement is not allowed.
		/// </summary>
		/// <typeparam name="Node"></typeparam>
		/// <param name="node1"></param>
		/// <param name="node2"></param>
		/// <returns></returns>
		private double ManhattanDistance<Node>(Node node1, Node node2)
			where Node : INode
		{
			return Math.Abs(node1.WorldGridIndex.X - node2.WorldGridIndex.X) + Math.Abs(node1.WorldGridIndex.Y - node2.WorldGridIndex.Y);
		}

		#endregion

	}
}
