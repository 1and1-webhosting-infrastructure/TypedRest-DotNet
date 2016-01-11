﻿using Caliburn.Micro;
using TypedRest.Samples.Library.Endpoints;
using TypedRest.Samples.Library.Models;
using TypedRest.Wpf.ViewModels;

namespace TypedRest.Samples.Wpf.ViewModels
{
    public class ResourceElementViewModel : ElementViewModel<Resource>
    {
        protected new readonly ResourceElement Endpoint;

        public ResourceElementViewModel(ResourceElement endpoint, IEventAggregator eventAggregator)
            : base(endpoint, eventAggregator)
        {
            Endpoint = endpoint;
        }

        public void ListRevisions()
        {
            ((IConductor)Parent).ActivateItem(new ResourceRevisionCollectionViewModel(Endpoint.Revisions, EventAggregator));
        }

        public void ListEvents()
        {
            ((IConductor)Parent).ActivateItem(new StreamViewModel<LogEvent>(Endpoint.Events, EventAggregator));
        }
    }
}