using project.App.Services;
using project.BL.Models;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace project.App.ViewModels.Student
{
    public partial class StudentDetailViewModel : ViewModelBase
    {

        public StudentDetailModel Student { get; set; }


        public StudentDetailViewModel(IMessengerService messengerService): base(messengerService) {
            Student = StudentDetailModel.Empty;
        }

    }
}
