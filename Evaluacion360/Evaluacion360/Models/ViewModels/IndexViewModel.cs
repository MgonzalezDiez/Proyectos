using Evaluacion360.Models;
using Evaluacion360.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Evaluacion360.Models.ViewModels
{
    public class IndexViewModel: BasePaginador
    {
        public List<UserIndexViewModel> Secciones { get; set; }
    }

    public class ListSectionViewModel: BasePaginador
    {
        public List<SectionListViewModel> Secciones { get; set; }
    }

    public class ListAEQViewModel : BasePaginador
    {
        public List<AutoEvaluationQuestionListModel> Secciones { get; set; }
    }

    public class ListEPViewModel : BasePaginador
    {
        public List<EvaluatorPositionListViewModel> Secciones { get; set; }
    }

    public class ListPositionViewModel : BasePaginador
    {
        public List<PositionListViewModel> Secciones { get; set; }
    }

    public class ListEvProcsViewModel : BasePaginador
    {
        public List<EvaluationProcessViewModel> Secciones { get; set; }
    }

    public class ListAutoEvaluationViewModel : BasePaginador
    {
        public List<AutoEvaluationListViewModel> Secciones { get; set; }
    }

    public class ListPositionEvaluationViewModel : BasePaginador
    {
        public List<PositionEvaluationsListViewModel> Secciones { get; set; }
    }

    public class ListEvaluationPositionsViewModel : BasePaginador
    {
        public List<EvaluationPositionsListViewModel> Secciones { get; set; }
    }
}