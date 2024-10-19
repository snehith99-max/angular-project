import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { NgxSpinnerService } from 'ngx-spinner';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import { FormControl, FormGroup, FormBuilder, Validators } from '@angular/forms';

@Component({
  selector: 'app-hrm-mst-subfunction',
  templateUrl: './hrm-mst-subfunction.component.html',
})

export class HrmMstSubfunctionComponent {
  txtsubfunction: any;
  subfunction_list: any;
  AddForm!: FormGroup;
  txteditsubfunction: any;
  txteditlms_code: any;
  txteditbureau_code: any;
  rbo_status: any;
  subfunctioninactivelog_list: any;
  EditForm!: FormGroup;
  StatusForm!: FormGroup;

  subfunction_gid = "";
  addsubfunctionFormData = { txtsubfunction: '', };
  editsubfunctionFormData = { txteditsubfunction: '', };
  statussubfunctionFormData = {
    txtsubfunction: '',
    remarks: '',
    rbo_status: ''
  };

  parametervalue: any;

  constructor(public router: Router, private ToastrService: ToastrService, private NgxSpinnerService: NgxSpinnerService, private service: SocketService, public FormBuilder: FormBuilder) {
    this.createForm();
    this.createEditForm();
    this.createStatusForm();
  }

  createForm() {
    this.AddForm = this.FormBuilder.group({
      txtsubfunction: ['', [Validators.required, Validators.pattern("^(?!\s*$).+")]],
    });
  }

  createEditForm() {
    this.EditForm = this.FormBuilder.group({
      txteditsubfunction: ['', [Validators.required, Validators.pattern("^(?!\s*$).+")]],
    });
  }

  createStatusForm() {
    this.StatusForm = this.FormBuilder.group({
      remarks: ['', [Validators.required, Validators.pattern("^(?!\s*$).+")]],
      rbo_status: ['']
    });
  }

  clearForm() {
    this.AddForm.reset();
  }

  // Summary
  ngOnInit() {
    var url = 'HrmMaster/GetSubFunction';
    this.NgxSpinnerService.show();
    this.service.get(url).subscribe((result: any) => {
      if (result.master_list != null) {
        $('#subfunctionsummary').DataTable().destroy();
        this.subfunction_list = result.master_list;
        this.NgxSpinnerService.hide();
        setTimeout(() => {
          $('#subfunctionsummary').DataTable();
        }, 1);
      }
      else {
        setTimeout(() => {
          $('#subfunctionsummary').DataTable();
        }, 1);
        this.subfunction_list = result.master_list;
        this.NgxSpinnerService.hide();
        $('#subfunctionsummary').DataTable().destroy();
      }
    });
  }

  get remarks() {
    return this.StatusForm.get('remarks')
  }

  opensubfunctionpopup() {
    this.clearForm();
  }
  // Add
  addsubfunction() {
    if (this.AddForm.valid) {
      var params = {
        subfunction_name: this.addsubfunctionFormData.txtsubfunction,
      }
      this.NgxSpinnerService.show();
      var url = 'HrmMaster/CreateSubFunction';
      this.NgxSpinnerService.hide();
      this.service.post(url, params).subscribe((result: any) => {
        if (result.status == true) {
          this.ToastrService.success(result.message)
          this.ngOnInit();
        }
        else {
          this.ToastrService.warning(result.message)
          this.NgxSpinnerService.hide();
        }
      })
    }
  }

  editsubfunction(subfunction_gid: any) {
    this.subfunction_gid = subfunction_gid;
    this.EditForm.reset();
    var param = {
      subfunction_gid: subfunction_gid
    }
    this.NgxSpinnerService.show();
    var url = 'HrmMaster/EditSubFunction';
    this.service.getparams(url, param).subscribe((result: any) => {

      this.editsubfunctionFormData.txteditsubfunction = result.subfunction_name;
      this.NgxSpinnerService.hide();
    });
  }

  updatesubfunction() {
    //Update the values in database
    var url = 'HrmMaster/UpdateSubFunction';
    var params = {
      subfunction_gid: this.subfunction_gid,
      subfunction_name: this.editsubfunctionFormData.txteditsubfunction,
    }
    this.NgxSpinnerService.show();
    this.service.post(url, params).subscribe((result: any) => {
      if (result.status == true) {
        this.NgxSpinnerService.hide();
        this.ToastrService.success(result.message);
        this.ngOnInit();
      }
      else {
        this.ToastrService.warning(result.message);
        this.NgxSpinnerService.hide();
      }
    })
  }

  Status_update(subfunction_gid: any) {
    this.StatusForm.reset();
    this.NgxSpinnerService.show();
    this.subfunction_gid = subfunction_gid
    var params = {
      subfunction_gid: subfunction_gid
    }
    var url = 'HrmMaster/EditSubFunction';
    this.service.getparams(url, params).subscribe((result: any) => {
      this.subfunction_gid = result.subfunction_gid;
      this.statussubfunctionFormData.txtsubfunction = result.subfunction_name;
      this.statussubfunctionFormData.rbo_status = result.Status;
      this.NgxSpinnerService.hide();
    });
    this.NgxSpinnerService.show();

    var url = 'HrmMaster/SubFunctionInactiveLogview'
    this.service.getparams(url, params).subscribe((result: any) => {
      this.subfunctioninactivelog_list = result.master_list;
    });
  }

  update_status() {
    var params = {
      subfunction_gid: this.subfunction_gid,
      subfunction_name: this.statussubfunctionFormData.txtsubfunction,
      remarks: this.statussubfunctionFormData.remarks,
      rbo_status: this.statussubfunctionFormData.rbo_status
    }

    this.NgxSpinnerService.show();
    var url = 'HrmMaster/InactiveSubFunction';
    this.service.post(url, params).subscribe((result: any) => {
      if (result.status == true) {
        this.NgxSpinnerService.hide();
        this.ToastrService.success(result.message);
        this.ngOnInit();
      }
      else {
        this.ToastrService.warning(result.message);
        this.NgxSpinnerService.hide();
      }
    })
  };

  delete(subfunction_gid: any) {
    this.parametervalue = subfunction_gid
  }

  ondelete() {
    this.NgxSpinnerService.show();
    var params = {
      subfunction_gid: this.parametervalue
    }
    var url = 'HrmMaster/DeleteSubFunction';
    this.service.getparams(url, params).subscribe((result: any) => {
      if (result.status == true) {
        this.NgxSpinnerService.hide();
        this.ToastrService.success("Sub Function Deleted Successfully");
        this.ngOnInit();
      }
      else {
        this.ToastrService.warning("Error Occurred While Deleting Sub Function");
        this.NgxSpinnerService.hide();
      }
    })
  }

  close() {
    window.location.reload();
  }
}