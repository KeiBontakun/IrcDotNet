﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;

namespace IrcDotNet
{
    using Common.Collections;

    /// <summary>
    /// Represents a collection of <see cref="IrcChannel"/> objects.
    /// </summary>
    /// <seealso cref="IrcChannel"/>
    public class IrcChannelCollection : ReadOnlyObservableCollection<IrcChannel>
    {
        private IrcClient client;

        internal IrcChannelCollection(IrcClient client, ObservableCollection<IrcChannel> list)
            : base(list)
        {
            this.client = client;
        }

        /// <summary>
        /// Gets the client to which the collection of channels belongs.
        /// </summary>
        /// <value>The client to which the collection of channels belongs.</value>
        public IrcClient Client
        {
            get { return this.client; }
        }

        /// <inheritdoc cref="Join(IEnumerable{string})"/>
        public void Join(params string[] channels)
        {
            Join((IEnumerable<string>)channels);
        }

        /// <inheritdoc cref="Join(IEnumerable{Tuple{string, string}})"/>
        /// <param name="channels">A collection of the names of channels to join.</param>
        public void Join(IEnumerable<string> channels)
        {
            this.Client.Join(channels);
        }

        /// <inheritdoc cref="Join(IEnumerable{Tuple{string, string}})"/>
        public void Join(params Tuple<string, string>[] channels)
        {
            Join((IEnumerable<Tuple<string, string>>)channels);
        }

        /// <summary>
        /// Joins the specified channels.
        /// </summary>
        /// <param name="channels">A collection of 2-tuples of the names of channels to join and their keys.</param>
        public void Join(IEnumerable<Tuple<string, string>> channels)
        {
            this.Client.Join(channels);
        }

        /// <inheritdoc cref="Leave(IEnumerable{string}, string)"/>
        public void Leave(params string[] channels)
        {
            Leave((IEnumerable<string>)channels);
        }

        /// <summary>
        /// Leaves the specified channels, giving the specified comment.
        /// </summary>
        /// <param name="channels">A collection of the names of channels to leave.</param>
        /// <param name="comment">The comment to send the server upon leaving the channel, or <see langword="null"/> for
        /// no comment.</param>
        public void Leave(IEnumerable<string> channels, string comment = null)
        {
            this.Client.Leave(channels, comment);
        }

        /// <summary>
        /// Raises the <see cref="ReadOnlyObservableCollection{T}.CollectionChanged"/> event.
        /// </summary>
        /// <param name="e">The <see cref="NotifyCollectionChangedEventArgs"/> instance containing the event data.</param>
        protected override void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
        {
            base.OnCollectionChanged(e);

            // Unset channel of all items removed from collection to null, and set channel of all items added to collection.
            if (e.OldItems != null)
                e.OldItems.Cast<IrcChannel>().ForEach(item => item.Client = null);
            if (e.NewItems != null)
                e.NewItems.Cast<IrcChannel>().ForEach(item => item.Client = this.client);
        }
    }
}
