
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Solution
{
    class Solution
    {
        static void Main(string[] args)
        {
            /* Enter your code here. Read input from STDIN. Print output to STDOUT */
            OrderManager orderManager = new OrderManager();
            orderManager.PopulateOrders();
            orderManager.ExecuteOrders();
        }
    }
    class OrderManager
    {
        public static IList<Order> _orderLine = new List<Order>();
        public static IList<Order> _stopOrderLine = new List<Order>();
        public static int orderNumber = 1;

        public void PopulateOrders()
        {
            //read from stdin 
            string orderStr = Console.ReadLine();
            while (!String.IsNullOrEmpty(orderStr))
            {
                //parse input string to Order
                Order curOrder = ParseOrder(orderStr);
                _orderLine.Add(curOrder);
                orderStr = Console.ReadLine();
            }
        }

        public void ExecuteOrders()
        {
            foreach (var order in _orderLine)
            {
                if (order.OrderType != OrderType.Stop)
                {
                    order.ExecuteOrder(_orderLine, _stopOrderLine);
                }
            }
        }

        private Order ParseOrder(string orderStr)
        {
            orderStr.Trim();
            String[] orderDetail = orderStr.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            Order order = null;
            //get order type
            OrderType curOrderType;
            Enum.TryParse(orderDetail[0], true, out curOrderType);
            switch (curOrderType)
            {
                case OrderType.Market:
                    order = new MarketOrder(orderNumber++, OrderType.Market, orderDetail[1],
                        long.Parse(orderDetail[2]), Double.Parse(orderDetail[3]));
                    break;
                case OrderType.Limit:
                    order = new LimitOrder(orderNumber++, OrderType.Limit, orderDetail[1],
                        long.Parse(orderDetail[2]), Double.Parse(orderDetail[3]));
                    break;
                case OrderType.Stop:
                    order = new StopOrder(orderNumber++, OrderType.Stop, orderDetail[1],
                        long.Parse(orderDetail[2]), Double.Parse(orderDetail[3]));
                    _stopOrderLine.Add(order);
                    break;
                case OrderType.Cancel:
                    order = new CancelOrder(orderNumber++, OrderType.Cancel, orderDetail[1],
                        long.Parse(orderDetail[2]), Double.Parse(orderDetail[3]));
                    break;
                default:
                    break;
            }
            return order;
        }
    }
    abstract class Order
    {
        public int OrderNumber { get; set; }
        public OrderType OrderType { get; set; }
        public string Side { get; set; }
        public long Value1 { get; set; }
        public double Value2 { get; set; }
        public OrderStatus OrderStatus { get; set; }

        public Order() { }
        public Order(int number, OrderType ot, string side, long v1, double v2)
        {
            this.OrderNumber = number;
            this.OrderType = ot;
            this.Side = side;
            this.Value1 = v1;
            this.Value2 = v2;
            this.OrderStatus = 0;
        }

        public abstract void ExecuteOrder(IList<Order> orderLine, IList<Order> stopOrderLine);

        protected bool IsStopTriggered(Order taker, Order maker, IList<Order> orderLine, IList<Order> stopOrderLine)
        {

            bool flag = false;
            var stopOrders = stopOrderLine.OrderBy(order => order.OrderNumber);
            foreach (var stopOrder in stopOrders)
            {
                if (taker.Side.ToLower() == "buy" && stopOrder.Side.ToLower() == "buy"
                        && maker.Value2 >= stopOrder.Value2)
                {
                    ((StopOrder)stopOrder).TriggerStop(taker, orderLine, stopOrderLine);
                    flag = true;
                }
                if (taker.Side.ToLower() == "sell" && stopOrder.Side.ToLower() == "sell"
                    && maker.Value2 <= stopOrder.Value2)
                {
                    ((StopOrder)stopOrder).TriggerStop(taker, orderLine, stopOrderLine);
                    flag = true;
                }
            }
            return flag;
        }

        protected void Trade(Order taker, Order maker)
        {
            long trade = Math.Min(taker.Value1, maker.Value1);
            taker.Value1 -= trade;
            maker.Value1 -= trade;
            //write out
            WriteOutMatch(taker.OrderNumber, maker.OrderNumber, trade, maker.Value2);
            //update status
            taker.OrderStatus = taker.Value1 == 0 ? OrderStatus.Executed : OrderStatus.PartialFilled;
            maker.OrderStatus = maker.Value1 == 0 ? OrderStatus.Executed : OrderStatus.PartialFilled;

        }

        protected void WriteOutMatch(int taker, int maker, long volume, double price)
        {
            Console.WriteLine("match {0} {1} {2} {3:F2}", taker, maker, volume, price);
        }
    }
    //save for later
    class Match
    {
        public int Taker { get; set; }
        public int Maker { get; set; }
        public int Volume { get; set; }
        public int Price { get; set; }
    }
    class MarketOrder : Order
    {
        public MarketOrder(int number, OrderType ot, string side, long v1, double v2)
            : base(number, ot, side, v1, v2) { }

        public override void ExecuteOrder(IList<Order> orderLine, IList<Order> stopOrderLine)
        {
            IEnumerable<Order> matchedOrders;
            //filter previous limit order(processing/partialfilled)
            matchedOrders = orderLine.Where(order => order.OrderNumber < this.OrderNumber)
                                .Where(o => o.OrderType == OrderType.Limit)
                                .Where(order => order.OrderStatus <= OrderStatus.PartialFilled);
            if (this.Side.ToLower() == "buy")
            {
                //find sell order, ascending sort by price
                matchedOrders = matchedOrders.Where(order => order.Side.ToLower() == "sell")
                    .OrderBy(order => order.Value2)
                    .ThenBy(order => order.OrderNumber);
            }
            else
            {
                //find buy order, descending sort by price
                matchedOrders = matchedOrders.Where(order => order.Side.ToLower() == "buy")
                    .OrderByDescending(order => order.Value2)
                    .ThenBy(order => order.OrderNumber);
            }
            if (matchedOrders != null)
            {
                foreach (var makerOrder in matchedOrders)
                {
                    if (this.OrderStatus <= OrderStatus.PartialFilled
                        && makerOrder.OrderStatus <= OrderStatus.PartialFilled)
                    {
                        Trade(this, makerOrder);

                        if (IsStopTriggered(this, makerOrder, orderLine, stopOrderLine))
                        {
                            break;
                        }
                    }
                }
            }
            this.OrderStatus = OrderStatus.Executed;
        }
    }
    class LimitOrder : Order
    {
        public LimitOrder(int number, OrderType ot, string side, long v1, double v2)
            : base(number, ot, side, v1, v2) { }

        public override void ExecuteOrder(IList<Order> orderLine, IList<Order> stopOrderLine)
        {
            IEnumerable<Order> matchedOrders;
            //filter previous limit order(processing/partialfilled) 
            matchedOrders = orderLine.Where(order => order.OrderNumber < this.OrderNumber)
                    .Where(o => o.OrderType == OrderType.Limit)
                    .Where(order => order.OrderStatus <= OrderStatus.PartialFilled);
            if (this.Side.ToLower() == "buy")
            {
                //ascending sort by price, buy-out
                matchedOrders = matchedOrders.Where(order => order.Side.ToLower() == "sell")
                    .Where(o => o.Value2 <= this.Value2)
                    .OrderBy(order => order.Value2)
                    .ThenBy(order => order.OrderNumber);
            }
            else
            {
                //descending sort by price, sell-out
                matchedOrders = matchedOrders.Where(order => order.Side.ToLower() == "buy")
                    .Where(o => o.Value2 >= this.Value2)
                    .OrderByDescending(order => order.Value2)
                    .ThenBy(order => order.OrderNumber);
            }
            if (matchedOrders != null)
            {
                foreach (var makerOrder in matchedOrders)
                {
                    if (this.OrderStatus <= OrderStatus.PartialFilled
                        && makerOrder.OrderStatus <= OrderStatus.PartialFilled)
                    {
                        Trade(this, makerOrder);

                        if (IsStopTriggered(this, makerOrder, orderLine, stopOrderLine))
                        {
                            break;
                        }
                    }
                }
            }
        }
    }
    class StopOrder : Order
    {
        public StopOrder(int number, OrderType ot, string side, long v1, double v2)
            : base(number, ot, side, v1, v2) { }

        public override void ExecuteOrder(IList<Order> orderLine, IList<Order> stopOrderLine)
        {
            //          
        }

        public void TriggerStop(Order taker, IList<Order> orderLine, IList<Order> stopOrderLine)
        {
            IEnumerable<Order> matchedOrders;
            //filter previous limit order(processing/partialfilled)
            matchedOrders = orderLine.Where(order => order.OrderNumber < taker.OrderNumber)
                                .Where(o => o.OrderType == OrderType.Limit)
                                .Where(order => order.OrderStatus <= OrderStatus.PartialFilled);
            if (this.Side.ToLower() == "buy")
            {
                //find sell order, ascending sort by price
                matchedOrders = matchedOrders.Where(order => order.Side.ToLower() == "sell")
                    .OrderBy(order => order.Value2)
                    .ThenBy(order => order.OrderNumber);
            }
            else
            {
                //find buy order, descending sort by price
                matchedOrders = matchedOrders.Where(order => order.Side.ToLower() == "buy")
                    .OrderByDescending(order => order.Value2)
                    .ThenBy(order => order.OrderNumber);
            }
            if (matchedOrders != null)
            {
                foreach (var makerOrder in matchedOrders)
                {
                    if (this.OrderStatus <= OrderStatus.PartialFilled
                        && makerOrder.OrderStatus <= OrderStatus.PartialFilled)
                    {
                        Trade(this, makerOrder);
                    }
                }
            }
            this.OrderStatus = OrderStatus.Executed;
        }
    }
    class CancelOrder : Order
    {
        public CancelOrder(int number, OrderType ot, string side, long v1, double v2)
            : base(number, ot, side, v1, v2) { }

        public override void ExecuteOrder(IList<Order> orderLine, IList<Order> stopOrderLine)
        {
            //find the target order 
            //if its status is Processing or PartialFilled set it to Canceled, otherwise no-op
            try
            {
                //list index is 1 lesser than order number(ID)
                int targetIndex = (int)this.Value1 - 1;
                OrderStatus currentStatus = orderLine[targetIndex].OrderStatus;
                if (currentStatus <= OrderStatus.PartialFilled)
                {
                    orderLine[targetIndex].OrderStatus = OrderStatus.Canceled;
                }
            }
            catch (ArgumentOutOfRangeException)
            {
            }
        }
    }
    enum OrderType
    {
        Market,
        Limit,
        Stop,
        Cancel
    }
    enum OrderStatus
    {
        Processing = 0,
        PartialFilled,
        Executed,
        Canceled
    }
}
