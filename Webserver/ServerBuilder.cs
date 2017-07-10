﻿using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;

using RequestList = System.Collections.Immutable.ImmutableList<(System.Text.RegularExpressions.Regex, System.Action<Jambox.Web.Request>)>;
using RequestListBuilder = System.Collections.Immutable.ImmutableList<(System.Text.RegularExpressions.Regex, System.Action<Jambox.Web.Request>)>.Builder;
namespace Jambox.Web
{
    /// <summary>
    /// Creates the server. You can get an instance of this in the Server class, with Server.New(...)
    /// </summary>
    public class ServerBuilder
    {
        Server ws;
        RequestListBuilder getRq;
        RequestListBuilder putRq;
        RequestListBuilder postRq;
        RequestListBuilder delRq;
        IPAddress ip;
        int port;
        RegexOptions caseSensitive;
        internal ServerBuilder(IPAddress _ip, int _port, RegexOptions caseSensitivity, string majorErrorString)
        {
            ws = new Server();
            ws.MajorErrorString = majorErrorString;
            ip = _ip;
            port = _port;
            getRq = System.Collections.Immutable.ImmutableList.CreateBuilder<(Regex, Action<Request>)>();
            putRq = System.Collections.Immutable.ImmutableList.CreateBuilder<(Regex, Action<Request>)>();
            postRq = System.Collections.Immutable.ImmutableList.CreateBuilder<(Regex, Action<Request>)>();
            delRq = System.Collections.Immutable.ImmutableList.CreateBuilder<(Regex, Action<Request>)>();
            caseSensitive = caseSensitivity;
        }
        /// <summary>
        /// Add a GET route to the server.
        /// </summary>
        /// <param name="pattern">Regex pattern for the route.</param>
        /// <param name="action">Action to perform for this route.</param>
        /// <returns>self, for chained method calls.</returns>
        public ServerBuilder GET(string pattern, Action<Request> action)
        {
            getRq.Add((new Regex(pattern, RegexOptions.Compiled | caseSensitive), action));
            return this;
        }
        /// <summary>
        /// Add a GET route to the server, with specified case-sensitivity.
        /// </summary>
        /// <param name="pattern">Regex pattern for the route.</param>
        /// <param name="action">Action to perform for this route.</param>
        /// <param name="caseSensitive">Is this case sensitive?</param>
        /// <returns>self, for chained method calls.</returns>
        public ServerBuilder GET(string pattern, Action<Request> action, bool caseSensitive)
        {
            getRq.Add((new Regex(pattern,
                RegexOptions.Compiled | (caseSensitive ? RegexOptions.IgnoreCase : RegexOptions.None)), action));
            return this;
        }
        /// <summary>
        /// Add a POST route to the server, with specified case-sensitivity.
        /// </summary>
        /// <param name="pattern">Regex pattern for the route.</param>
        /// <param name="action">Action to perform for this route.</param>
        /// <returns>self, for chained method calls.</returns>
        public ServerBuilder POST(string pattern, Action<Request> action)
        {
            postRq.Add((new Regex(pattern, RegexOptions.Compiled | caseSensitive), action));
            return this;
        }
        /// <summary>
        /// Add a POST route to the server, with specified case-sensitivity.
        /// </summary>
        /// <param name="pattern">Regex pattern for the route.</param>
        /// <param name="action">Action to perform for this route.</param>
        /// <param name="caseSensitive">Is this case sensitive?</param>
        /// <returns>self, for chained method calls.</returns>
        public ServerBuilder POST(string pattern, Action<Request> action, bool caseSensitive)
        {
            postRq.Add((new Regex(pattern,
                RegexOptions.Compiled | (caseSensitive ? RegexOptions.IgnoreCase : RegexOptions.None)), action));
            return this;
        }
        /// <summary>
        /// Add a PUT route to the server, with specified case-sensitivity.
        /// </summary>
        /// <param name="pattern">Regex pattern for the route.</param>
        /// <param name="action">Action to perform for this route.</param>
        /// <returns>self, for chained method calls.</returns>
        public ServerBuilder PUT(string pattern, Action<Request> action)
        {
            putRq.Add((new Regex(pattern, RegexOptions.Compiled | caseSensitive), action));
            return this;
        }
        /// <summary>
        /// Add a PUT route to the server, with specified case-sensitivity.
        /// </summary>
        /// <param name="pattern">Regex pattern for the route.</param>
        /// <param name="action">Action to perform for this route.</param>
        /// <param name="caseSensitive">Is this case sensitive?</param>
        /// <returns>self, for chained method calls.</returns>
        public ServerBuilder PUT(string pattern, Action<Request> action, bool caseSensitive)
        {
            putRq.Add((new Regex(pattern,
                RegexOptions.Compiled | (caseSensitive ? RegexOptions.IgnoreCase : RegexOptions.None)), action));
            return this;
        }
        /// <summary>
        /// Add a DELETE route to the server, with specified case-sensitivity.
        /// </summary>
        /// <param name="pattern">Regex pattern for the route.</param>
        /// <param name="action">Action to perform for this route.</param>
        /// <returns>self, for chained method calls.</returns>
        public ServerBuilder DELETE(string pattern, Action<Request> action)
        {
            delRq.Add((new Regex(pattern, RegexOptions.Compiled | caseSensitive), action));
            return this;
        }
        /// <summary>
        /// Add a DELETE route to the server, with specified case-sensitivity.
        /// </summary>
        /// <param name="pattern">Regex pattern for the route.</param>
        /// <param name="action">Action to perform for this route.</param>
        /// <param name="caseSensitive">Is this case sensitive?</param>
        /// <returns>self, for chained method calls.</returns>
        public ServerBuilder DELETE(string pattern, Action<Request> action, bool caseSensitive)
        {
            delRq.Add((new Regex(pattern,
                RegexOptions.Compiled | (caseSensitive ? RegexOptions.IgnoreCase : RegexOptions.None)), action));
            return this;
        }
        /// <summary>
        /// Construct the server based on the current state of the ServerBuilder.
        /// </summary>
        /// <returns>A server.</returns>
        public Server Build()
        {
            ws.getRouteMap = getRq.ToImmutable();
            ws.postRouteMap = postRq.ToImmutable();
            ws.putRouteMap = putRq.ToImmutable();
            ws.deleteRouteMap = delRq.ToImmutable();
            ws.tcp = new TcpListener(ip, port);
            return ws;
        }
    }
}
