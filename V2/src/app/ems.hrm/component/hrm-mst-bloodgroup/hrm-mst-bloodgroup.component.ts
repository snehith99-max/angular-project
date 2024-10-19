import { Component, ElementRef } from '@angular/core';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import { NgxSpinnerService } from 'ngx-spinner';
import { ToastrService } from 'ngx-toastr';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';

@Component({
  selector: 'app-hrm-mst-bloodgroup',
  templateUrl: './hrm-mst-bloodgroup.component.html',
})

export class HrmMstBloodgroupComponent {
  // addFormData = { bloodgroup_name: '', };
  editFormData = { txteditblood_group: '', };
  statusFormData = {
    bloodgroup_gid: '',
    rbo_status: '',
    txtblood_group: '',
    txtremarks: ''
  };

  bloodgroup_list: any;
  txtblood_group: any;
  // txteditblood_group: any;
  bloodgroup_gid: any;
  rbo_status: any;
  bloodgroupinactivelog_list: any;
  BldAddForm: FormGroup | any;
  bldeditForm: FormGroup | any;
  stsform: FormGroup | any;
  txtblood_add: any;
  txt_blood_add: any;
  parametervalue: any;
  responsedata: any;

  constructor(private SocketService: SocketService, private el: ElementRef, private NgxSpinnerService: NgxSpinnerService, private ToastrService: ToastrService, private FormBuilder: FormBuilder, public service: SocketService) {
    // this.createeAddForm();
    this.createbldeditForm();
    this.createstsform();
  }

  // createeAddForm() {
  //   this.BldAddForm = this.FormBuilder.group({
  //     bloodgroup_name: new FormControl('', [Validators.required, Validators.pattern(/^(?!\s*$).+/)]),
  //   });
  // }

  createbldeditForm() {
    this.bldeditForm = this.FormBuilder.group({
      txteditblood_group: new FormControl('', [Validators.required, Validators.pattern(/^(?!\s*$).+/)]),
    });
  }

  createstsform() {
    this.stsform = this.FormBuilder.group({
      txtremarks: new FormControl('', [Validators.required, Validators.pattern(/^(?!\s*$).+/)]),
      rbo_status: ['']
    });
  }
  clearForm() {
    this.BldAddForm.reset();
  }

  openaddpopup() {
    this.BldAddForm.reset();
  }

  ngOnInit() {
    this.GetBloodGroupSummary();
    // Form values for Add popup//
    this.BldAddForm = new FormGroup({
      bloodgroup_name: new FormControl('', [Validators.required, Validators.pattern(/^\S.*$/)])
    });

  } 

  GetBloodGroupSummary() {
    var url = 'HrmMaster/GetBloodGroupActive'
    this.service.get(url).subscribe((result: any) => {
      $('#bloodgroup_list').DataTable().destroy();
      this.responsedata = result;
      this.bloodgroup_list = this.responsedata.master_list;
      setTimeout(() => {
        $('#bloodgroup_list').DataTable();
      },);
    });
  }

  openpopup() {
    this.clearForm();
  }

  get txtremarks(){
    return this.stsform.get('txtremarks')
  }

  addbloodgroup() {
    debugger;
    if (this.BldAddForm.value.bloodgroup_name != null && this.BldAddForm.value.bloodgroup_name != '') {
      this.BldAddForm.value;
    var url = 'HrmMaster/CreateBloodGroup';
    this.service.postparams(url,  this.BldAddForm.value).pipe().subscribe(result => {
      if (result.status == false) {
        this.GetBloodGroupSummary();
          this.ToastrService.warning(result.message)
          this.BldAddForm.reset();
        
      }
      else {
        this.GetBloodGroupSummary();
        this.ToastrService.success(result.message)
        this.BldAddForm.reset();
         }
    })
  }
  else {
    this.ToastrService.warning("Kindly fill the mandatory fields")
  }
  this.BldAddForm.reset();
  this.GetBloodGroupSummary();
  }

  editbloodgroup(bloodgroup_gid: any) {
    this.NgxSpinnerService.show();
    var params = {
      bloodgroup_gid: bloodgroup_gid
    }
    var url = 'HrmMaster/EditBloodGroup';
    this.SocketService.getparams(url, params).subscribe((result: any) => {
      this.editFormData.txteditblood_group = result.bloodgroup_name;
      this.bloodgroup_gid = result.bloodgroup_gid;
    });
    this.NgxSpinnerService.hide();
  }

  update() {
    this.NgxSpinnerService.show();
    var url = 'HrmMaster/UpdateBloodGroup';
    var params = {
      bloodgroup_name: this.editFormData.txteditblood_group,
      bloodgroup_gid: this.bloodgroup_gid
    }
    this.SocketService.post(url, params).subscribe((result: any) => {
      if (result.status == true) {
        this.NgxSpinnerService.hide();
        this.ToastrService.success(result.message);
      }
      else {
        this.ToastrService.warning(result.message);
        this.NgxSpinnerService.hide();
      }
      this.ngOnInit();
      this.txtblood_group = null;
    })
  }

  Status_update(bloodgroup_gid: any) {
    this.bloodgroup_gid = bloodgroup_gid
    this.stsform.reset();
    var params = {
      bloodgroup_gid: bloodgroup_gid
    }
    this.NgxSpinnerService.show();
    var url = 'HrmMaster/EditBloodGroup';
    this.SocketService.getparams(url, params).subscribe((result: any) => {
      this.bloodgroup_gid = result.bloodgroup_gid
      this.statusFormData.txtblood_group = result.bloodgroup_name;
      this.statusFormData.rbo_status = result.Status;
      this.NgxSpinnerService.hide();
    });
    var url = 'HrmMaster/BloodGroupInactiveLogview'
    this.SocketService.getparams(url, params).subscribe((result: any) => {
      this.bloodgroupinactivelog_list = result.master_list;
    });
  }

  update_status() {
    this.NgxSpinnerService.show();
    var params = {
      bloodgroup_gid: this.bloodgroup_gid,
      remarks: this.statusFormData.txtremarks,
      rbo_status: this.statusFormData.rbo_status
    }

    var url = 'HrmMaster/InactiveBloodGroup';
    this.SocketService.post(url, params).subscribe((result: any) => {
      console.log(result.status)
      this.NgxSpinnerService.hide();
      if (result.status == true) {
        this.ToastrService.success(result.message)
        this.NgxSpinnerService.hide();
      }
      else {
        this.ToastrService.info(result.message)
      }
      this.ngOnInit()
    })
  };

  delete(bloodgroup_gid: any) {
    this.parametervalue = bloodgroup_gid
  }

  ondelete() {
    this.NgxSpinnerService.show();
    var params = {
      bloodgroup_gid: this.parametervalue
    }

    var url = 'HrmMaster/DeleteBloodGroup';
    this.SocketService.getparams(url, params).subscribe((result: any) => {
      if (result.status == true) {
        this.NgxSpinnerService.hide();
        this.ToastrService.success(" Blood Group Deleted Successfully");
        this.ngOnInit();
      }
      else {
        this.ToastrService.warning("Error Occurred While Deleting Blood Group!");
        this.NgxSpinnerService.hide();
      }
    })
  }

  close() {
  }
}