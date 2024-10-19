import { Component } from '@angular/core';
import { FormControl, FormControlName, FormGroup, Validators } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import { NgxSpinnerService } from 'ngx-spinner';
import flatpickr from 'flatpickr';
import { escapeSelector } from 'jquery';
import { ActivatedRoute, Router } from '@angular/router';


interface TeamDropdownItem {
  campaign_gid: string;
  // Add other properties as needed
}

@Component({
  selector: 'app-crm-trn-appointmentmanagement',
  templateUrl: './crm-trn-appointmentmanagement.component.html',
  styleUrls: ['./crm-trn-appointmentmanagement.component.scss'],
  styles: [`
  table thead th, 
  .table tbody td { 
   position: relative; 
  z-index: 0;
  } 
  .table thead th:last-child, 
  
  .table tbody td:last-child { 
   position: sticky; 
  
  right: 0; 
   z-index: 0; 
  
  } 
  .table td:last-child, 
  
  .table th:last-child { 
  
  padding-right: 50px; 
  
  } 
  .table.table-striped tbody tr:nth-child(odd) td:last-child { 
  
   background-color: #ffffff; 
    
    } 
    .table.table-striped tbody tr:nth-child(even) td:last-child { 
     background-color: #f2fafd; 
  
  } 
  `]
})
export class CrmTrnAppointmentmanagementComponent {
  reactiveForm!: FormGroup;
  GetLeaddropdown_list: any[] = [];
  leadbank_gid: any;
  leadbank_details: any = null;
  Getbussinessverticledropdown_list: any[] = [];
  GetAppointmentsummary_list: any[] = [];
  GetAppointmentTiles_list: any[] = [];
  total_unassigned: any;
  total_assigned: any;
  total_team: any;
  total_appointment: any;
  AssignForm!: FormGroup;
  GetTeamdropdown_list: any;
  inverted_dict: any;
  executive_list: any;
  EditreactiveForm!: FormGroup;
  editappointment_list: any;
  appointment_datepop: any;
  lead_titlepop: any;
  leadbank_namepop: any;
  showOptionsDivId: any;
  constructor(private ToastrService: ToastrService, public service: SocketService, private router: Router, private NgxSpinnerService: NgxSpinnerService) { }


  ngOnInit(): void {

    this.GetAppointmentsummary();
    this.GetAppointmentTiles();
    this.reactiveForm = new FormGroup({
      appointment_timing: new FormControl(''),
      leadname_gid: new FormControl(null),
      bussiness_verticle: new FormControl(null),
      remarks: new FormControl(null),
      lead_title: new FormControl(null,[Validators.required,
      Validators.pattern(/^(\S+\s*)*(?!\s).*$/)]),
    });
    this.AssignForm = new FormGroup({
      teamname_gid: new FormControl(null),
      employee_gid: new FormControl(null),
      appointment_gid: new FormControl(null),
    });
    this.EditreactiveForm = new FormGroup({
      editappointment_timing: new FormControl(''),
      editleadname_gid: new FormControl(null),
      editbussiness_verticle: new FormControl(null),
      editremarks: new FormControl(null),
      editlead_title: new FormControl([Validators.required,
      Validators.pattern(/^(\S+\s*)*(?!\s).*$/)]),
      appointment_gid: new FormControl(''),
    });

    this.GetLeaddropdown();
    this.Getbussinessverticledropdown();
    const today = new Date();
    const minDate = today;
    const Options = {
      enableTime: true,
      dateFormat: 'Y-m-d H:i:S',
      minDate: minDate,
      defaultDate: today,
      minuteIncrement: 1
    };
    flatpickr('.date-picker', Options);
    this.GetTeamdropdown();
    document.addEventListener('click', (event: MouseEvent) => {
      if (event.target && !(event.target as HTMLElement).closest('button.btn') && this.showOptionsDivId) {
        this.showOptionsDivId = null;
      }
    });
  }

  GetLeaddropdown() {
    var api = 'AppointmentManagement/GetLeaddropdown';
    this.service.get(api).subscribe((result: any) => {
      this.GetLeaddropdown_list = result.GetLeaddropdown_list;
    });
  }

  lead_details() {
    if (this.reactiveForm.value.leadname_gid !== null) {
      this.leadbank_gid = this.reactiveForm.value.leadname_gid;
    }
    else {

      this.leadbank_gid = this.EditreactiveForm.value.editleadname_gid;
    }

    const selectedLead = this.GetLeaddropdown_list.find(item => item.leadbank_gid === this.leadbank_gid);
    if (selectedLead) {
      const detailsArray = [
        selectedLead.leadbankbranch_name,
        selectedLead.leadbankcontact_name,
        selectedLead.address1,
        selectedLead.address2,
        selectedLead.city,
        selectedLead.state,
        selectedLead.pincode,
        selectedLead.mobile,
        selectedLead.email
      ];
      this.leadbank_details = detailsArray.filter(detail => detail).join('\n');

    }
  }
  Getbussinessverticledropdown() {
        var api = 'AppointmentManagement/Getbussinessverticledropdown';
        this.service.get(api).subscribe((result: any) => {
          this.Getbussinessverticledropdown_list = result.Getbussinessverticledropdown_list;
        });
  }
  popmodal(parameter: string, parameter1: string,parameter2: string) {
    this.lead_titlepop = parameter
    this.appointment_datepop = parameter1;
    this.leadbank_namepop = parameter2;
  } 
  

  get appointment_timing() {
    return this.reactiveForm.get('appointment_timing')!;
  }
  get leadname_gid() {
    return this.reactiveForm.get('leadname_gid')!;
  }
  get bussiness_verticle() {
    return this.reactiveForm.get('bussiness_verticle')!;
  }
  get lead_title() {
    return this.reactiveForm.get('lead_title')!;
  }
  get teamname_gid() {
    return this.AssignForm.get('teamname_gid')!;
  }
  get editappointment_timing() {
    return this.EditreactiveForm.get('editappointment_timing')!;
  }
  get editleadname_gid() {
    return this.EditreactiveForm.get('editleadname_gid')!;
  }
  get editbussiness_verticle() {
    return this.EditreactiveForm.get('editbussiness_verticle')!;
  }
  get editlead_title() {
    return this.EditreactiveForm.get('editlead_title')!;
  }
  onshopifyopen(){
    this.router.navigate(['/crm/CrmTrnShopifycontactus'])  }

  Onsubmit() {
    if (this.reactiveForm.value.appointment_timing != null &&
      this.reactiveForm.value.leadname_gid != null &&
      this.reactiveForm.value.bussiness_verticle != null &&
      this.reactiveForm.value.lead_title != null) {
      this.NgxSpinnerService.show();
      var url = 'AppointmentManagement/PostAppointment';
      this.service.post(url, this.reactiveForm.value).subscribe((result: any) => {
        if (result.status == false) {
          window.scrollTo({
            top: 0,
          });
          this.reactiveForm.reset();
          this.leadbank_details=null;
          this.NgxSpinnerService.hide();
          this.ToastrService.warning(result.message);
          this.GetAppointmentsummary();
          this.GetAppointmentTiles();
        }
        else {
          window.scrollTo({
            top: 0,
          });
          this.reactiveForm.reset();
          this.leadbank_details=null;
          this.NgxSpinnerService.hide();
          this.ToastrService.success(result.message);
          this.GetAppointmentsummary();
          this.GetAppointmentTiles();
        }
      });
    }
    else {
      this.ToastrService.warning('Kindly Fill All Mandatory Fields !! ')
    }
  }

  GetAppointmentsummary() {
    var api = 'AppointmentManagement/GetAppointmentsummary';
    this.service.get(api).subscribe((result: any) => {
      $('#GetAppointmentsummary_list').DataTable().destroy();
      this.GetAppointmentsummary_list = result.GetAppointmentsummary_list;
      setTimeout(() => {
        $('#GetAppointmentsummary_list').DataTable();
      }, 1);
    });
  }
  GetAppointmentTiles() {
    var api = 'AppointmentManagement/GetAppointmentTiles';
    this.service.get(api).subscribe((result: any) => {
      this.GetAppointmentTiles_list = result.GetAppointmentTiles_list;
      this.total_appointment = this.GetAppointmentTiles_list[0].total_appointment;
      this.total_team = this.GetAppointmentTiles_list[0].total_team;
      this.total_assigned = this.GetAppointmentTiles_list[0].total_assigned;
      this.total_unassigned = this.GetAppointmentTiles_list[0].total_unassigned;
    });
  }
  GetTeamdropdown() {
    var api = 'Assignvisit/Getmarketingteamdropdown';
    this.service.get(api).subscribe((result: any) => {
      this.GetTeamdropdown_list = result.marketingteamdropdown_list;
    });
  }
  marketingteam() {
    debugger
    let campaign_gid = this.AssignForm.get("teamname_gid")?.value;

    let params = {
      campaign_gid: campaign_gid
    }
    var url = 'Assignvisit/Getmarketingteamdropdownonchange'

    this.service.getparams(url, params).subscribe((result: any) => {

      this.executive_list = result.Getexecutedropdown;

    });
  }
  appointgid(params: any) {
    this.AssignForm.get('appointment_gid')?.setValue(params);
  }
  onadd() {
    this.router.navigate(['/crm/CrmTrnCreateopportunity'])

  }
  onadd1() {
    this.router.navigate(['/crm/CrmTrnIndiamartenquiry'])

  }
  public onupdate(): void {
    if (this.AssignForm.value.teamname_gid != null) {
      this.NgxSpinnerService.show();
      var url = 'AppointmentManagement/PostAssignedEmployee'
      this.service.post(url, this.AssignForm.value).pipe().subscribe((result: any) => {
        if (result.status == false) {
          window.scrollTo({
            top: 0,
          });
          this.ToastrService.warning(result.message)
          this.GetAppointmentsummary();
          this.GetAppointmentTiles();
          this.AssignForm.reset();
        }
        else {
          window.scrollTo({
            top: 0,
          });
          this.ToastrService.success(result.message)
          this.GetAppointmentsummary();
          this.GetAppointmentTiles();
          this.AssignForm.reset();
        }

        this.NgxSpinnerService.hide();
        window.location.reload();
      });
    }
    else {
      window.scrollTo({

        top: 0, // Code is used for scroll top after event done

      });
      this.ToastrService.warning('Kindly Fill All Mandatory Fields !! ')
    }
  }
  editappointment(appointment_gid: any) {
    this.editappointment_list = this.GetAppointmentsummary_list.find(item => item.appointment_gid === appointment_gid);
    this.EditreactiveForm.get('editappointment_timing')?.setValue(this.editappointment_list.fullformat_date);
    this.EditreactiveForm.get('editbussiness_verticle')?.setValue(this.editappointment_list.business_vertical);
    this.EditreactiveForm.get('editleadname_gid')?.setValue(this.editappointment_list.leadbank_gid);
    this.EditreactiveForm.get('editlead_title')?.setValue(this.editappointment_list.lead_title);

    this.leadbank_gid = this.EditreactiveForm.value.editleadname_gid;
    const selectedLead = this.GetLeaddropdown_list.find(item => item.leadbank_gid === this.leadbank_gid);
    if (selectedLead) {
      const detailsArray = [
        selectedLead.leadbankbranch_name,
        selectedLead.leadbankcontact_name,
        selectedLead.address1,
        selectedLead.address2,
        selectedLead.city,
        selectedLead.state,
        selectedLead.pincode,
        selectedLead.mobile,
        selectedLead.email
      ];
      this.leadbank_details = detailsArray.filter(detail => detail).join('\n');

    }
  }
  public editsubmit(): void {
    this.EditreactiveForm.get('appointment_gid')?.setValue(this.editappointment_list.appointment_gid);
    var url = 'AppointmentManagement/Posteditappointment'
    this.service.post(url, this.EditreactiveForm.value).pipe().subscribe((result: any) => {
      if (result.status == false) {
        window.scrollTo({
          top: 0,
        });
        this.ToastrService.warning(result.message)
        this.GetAppointmentsummary();
        this.GetAppointmentTiles();
      }
      else {
        window.scrollTo({
          top: 0,
        });
        this.ToastrService.success(result.message)
        this.GetAppointmentsummary();
        this.GetAppointmentTiles();
      }

      this.NgxSpinnerService.hide();
    });

  }
  onclose(){
    this.reactiveForm.reset();
    this.leadbank_details=null;
  }
  toggleOptions(appointment_gid: any) {
    if (this.showOptionsDivId === appointment_gid) {
      this.showOptionsDivId = null;
    } else {
      this.showOptionsDivId = appointment_gid;
    }
  }
}
