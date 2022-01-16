using System;
using System.Collections.Generic;

namespace FCmsManagerAngular.ViewModels {
    public class ContentRequestModel {

        public Guid repositoryid { get; set; }
        public List<FCms.Content.IContentFilter> filters { get; set; }
    };
}