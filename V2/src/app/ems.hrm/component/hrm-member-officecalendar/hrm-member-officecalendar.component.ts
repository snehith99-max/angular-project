import { Component, ViewChild, ElementRef,HostListener } from '@angular/core';
import { FormBuilder, FormGroup, FormControl, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import { ToastrService } from 'ngx-toastr';
import { FullCalendarComponent } from '@fullcalendar/angular';
import dayGridPlugin from '@fullcalendar/daygrid';
import { NgxSpinnerService } from 'ngx-spinner';
import { NgbModal, NgbModalRef } from '@ng-bootstrap/ng-bootstrap';
import flatpickr from 'flatpickr';
import { Options } from 'flatpickr/dist/types/options';

@Component({
  selector: 'app-hrm-member-officecalendar',
  templateUrl: './hrm-member-officecalendar.component.html',
  styleUrls: ['./hrm-member-officecalendar.component.scss']
})
export class HrmMemberOfficecalendarComponent {
  @ViewChild(FullCalendarComponent) calendarComponent !: FullCalendarComponent;
  @ViewChild('eventDetailPopup') eventDetailPopup: any;
  event: any;
  event_list: any[] = [];
  createevent: any[] = [];
  getData2: any;
  sidebarWidth: number = 0;
  calendarWidth: number = 0;
  createeventform!: FormGroup;
  updateEventForm!: FormGroup;
  sideNavExpanded: boolean = false;
  
  calendarOptions = {
    initialView: 'dayGridMonth',
    plugins: [dayGridPlugin],
    events: this.getEventsMethod(),
    eventContent: this.customizeEventContent.bind(this),
    eventClick: this.handleEventClick.bind(this),
    height: '500', 
    themeSystem: 'standard', 
    contentHeight: 'auto', 
    navLinks: true,
    editable: true,
    eventLimit: true,
    
  };
  responsedata: any;
  modalRef: NgbModalRef | undefined;
  monthlyreport: any;
  holidaycalender_list: any;
  constructor(private fb: FormBuilder, private route: ActivatedRoute, private router: Router, private service: SocketService, private ToastrService: ToastrService, private NgxSpinnerService: NgxSpinnerService, private modalService: NgbModal) {
    this.createeventform = new FormGroup({
      event_date: new FormControl('', [Validators.required]),
      event_time: new FormControl('', [Validators.required]),
      event_title: new FormControl('', [Validators.required])
    });

    this.updateEventForm = new FormGroup({
      event_date: new FormControl('', [Validators.required]),
      event_time: new FormControl('', [Validators.required]),
      event_title: new FormControl('', [Validators.required]),
      reminder_gid: new FormControl('')
    });

  }
  getEventsMethod() {
    return this.fetchEvents.bind(this);
  }

  customizeEventContent(arg: any) {
    const event = arg.event;
    // const eventTitle = event.title;
    const eventTitle = event.title.length > 20 ? event.title.substring(0, 20) + '...' : event.title; // Limit to 20 characters
    const eventTime = event.start.toLocaleTimeString([], { hour: '2-digit', minute: '2-digit', hour12: false }); // Format time (HH:MM)
    const eventDate = new Date(event.start).toISOString().split('T')[0];
    const reminder_gid = event.reminder_gid;
    const eventElement = document.createElement('div');
     let showDeleteIcon = false;

    eventElement.innerHTML = `
     <div class="event-content">
    <div style=" color: white; padding: 5px; display: flex; align-items: center; height: 100%; width: 100%; margin-right: 10px;  border-radius: 5px; ">
    <div style="width: 10px; height: 10px; background-color: black; border-radius: 50%; margin-right: 10px;"></div>
    <div style="flex: 1;">
    <div style="margin: 0; white-space: nowrap; overflow: hidden; text-overflow: ellipsis; color:black;">${eventTitle}</div>
      <div style="margin: 0; color:black;">${eventTime}</div>
    </div>
    <button title="Update Event" type="button" class="btn btn-icon bg-white me-2" style="padding: 5px; font-size: 7px;">
    <i class="fa-solid fa-arrow-right-from-bracket" style="color:black;"></i>
  </button>
  </div>
   </div>
`;

    eventElement.classList.add('custom-event');

   
    return { domNodes: [eventElement] };
  }
  
  handleEventClick(arg:any){
    this.event = arg.event; 
    const eventDate = new Date(this.event.start).toISOString().split('T')[0];
    const eventTime = this.event.start.toLocaleTimeString([], { hour: '2-digit', minute: '2-digit', hour12: false });
    const eventTitle = this.event.title;
    const reminderGid = this.event.extendedProps.reminder_gid;

    this.updateEventForm.patchValue({
      event_title: eventTitle,
      event_date: eventDate,
      event_time: eventTime,
      reminder_gid: reminderGid
  });

  // Open your modal here
  (<any>$('#updateEventModal')).modal('show');
  }
 

  ngOnInit(): void {
    var api = 'hrmTrnDashboard/holidaycalender';
    this.service.get(api).subscribe((result: any) => {
      this.responsedata = result;
      this.holidaycalender_list = this.responsedata.holidaycalender_list;
      setTimeout(() => {
        $('#office-calendar').DataTable();
      }, 1);
    });

    const options: Options = {
      dateFormat: 'd-m-Y',
      enableTime: true,
    };
    flatpickr('.date-picker', options);

  }
  
  
  

  fetchEvents(info: any, successCallback: any, failureCallback: any) {
    debugger;
    const url = 'hrmTrnDashboard/todayactivity';
    this.service.get(url).subscribe((result: any) => {
      this.responsedata = result;
      this.event_list = this.responsedata.createevent;
      console.log(this.event_list)
      if (this.event_list && this.event_list.length > 0) {
        const events = this.event_list.map((event: any) => ({
          title: event.event_title,
          start: this.formatDateTime(event.event_date, event.time),
          reminder_gid:event.reminder_gid,
        }));
       
        successCallback(events);
      } else {
        failureCallback('No events found');
      }
    });
  
  }
  formatDateTime(date: string, time: string): string {
    const [day, month, year] = date.split('-').map(Number);
    const [hour, minute] = time.split(':').map(Number);
    const isoDateTime = new Date(year, month - 1, day, hour, minute).toISOString();
    return isoDateTime;
  }

  createeventsubmit() {
    debugger
    let params = {
      event_date: this.createeventform.value.event_date,
      event_time: this.createeventform.value.event_time,
      event_title: this.createeventform.value.event_title,
    }
    var url = 'hrmTrnDashboard/postevent';
    this.service.post(url, params).subscribe((result: any) => {
      if (result.status == true) {
        this.ToastrService.success(result.message)
        
        this.calendarComponent.getApi().refetchEvents();
        this.createeventform.reset();
        this.NgxSpinnerService.hide();
      }
      else {
        this.ToastrService.warning(result.message)
        this.createeventform.reset();
        this.NgxSpinnerService.hide();
      }
    });
  }
  updateEvent() {
    debugger
    var url="hrmTrnDashboard/updateevent";
   this.service.postparams(url,this.updateEventForm.value).subscribe((result:any)=>{
      if(result.status == true){
        this.ToastrService.success(result.message)
        this.calendarComponent.getApi().refetchEvents();
        this.updateEventForm.reset();
        this.NgxSpinnerService.hide();
      }
      else{
        this.ToastrService.warning(result.message)
        this.updateEventForm.reset();
        this.NgxSpinnerService.hide();
      }
    });
  }
  deleteevent(reminder_gid:any){
    var url = "hrmTrnDashboard/deleteevent"
    this.NgxSpinnerService.show()
    var params={
      reminder_gid:reminder_gid
    }
    this.service.getparams(url,params).subscribe((result:any)=>{
      if(result.status==true){
        this.ToastrService.success(result.message)
        this.calendarComponent.getApi().refetchEvents();
        this.NgxSpinnerService.hide()
      }
      else{
        this.ToastrService.warning(result.message)
        this.NgxSpinnerService.hide()
      }
    });
  }
  back() {
    this.router.navigate(['/hrm/HrmMemberDashboard'])
  }
}
