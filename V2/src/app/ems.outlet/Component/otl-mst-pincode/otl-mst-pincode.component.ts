import { Component } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import { ToastrService } from 'ngx-toastr';
import { NgxSpinnerService } from 'ngx-spinner';

@Component({
  selector: 'app-otl-mst-pincode',
  templateUrl: './otl-mst-pincode.component.html',
  styleUrls: ['./otl-mst-pincode.component.scss']
})

export class OtlMstPincodeComponent {
  showOptionsDivId: any;
  reactiveForm!: FormGroup;
  pincode_list: any[] = [];
  Getoutletbranchuser_list: any[] = [];
  parameterValue: any;
  Mdloutlet: any;
  remainingChars: any | number = 6;

  constructor(public service: SocketService,
    private route: Router,
    private ToastrService: ToastrService,
    public NgxSpinnerService: NgxSpinnerService,) {
  }

  ngOnInit() {

    this.reactiveForm = new FormGroup({
      pincode: new FormControl('', [Validators.required,
      Validators.pattern("^(?!\\s*$)[a-zA-Z0-9\\s]*$")]),
      branch_gid: new FormControl(''),
      deliverycost: new FormControl(''),
    });

    var outletapi = 'Pincode/BranchDetailsOutlet';
    this.service.get(outletapi).subscribe((result: any) => {
      this.Getoutletbranchuser_list = result.Getpincode_list;
    });

    this.GetPincode();
  }
  get pincode() {
    return this.reactiveForm.get('pincode')!;
  }
  GetPincode() {
    var summaryapi = 'Pincode/GetPincodeSummary';
    this.service.get(summaryapi).subscribe((result: any) => {
      this.pincode_list = result.Getpincode_list;
      setTimeout(() => {
        $('#pincode_list').DataTable();
      }, 10);
    });
  }

  onupdatereset() {
    var postapi = 'Pincode/PostPincode';
    this.service.post(postapi, this.reactiveForm.value).subscribe((result: any) => {
      if (result.status == false) {
        this.ToastrService.warning('Error while adding pincode.');
      }
      else {
        this.GetPincode();
        this.reactiveForm.reset();
        this.ToastrService.success('Pincode added successfully.');
      }
    });
  }

  onclose() {
    this.reactiveForm.reset();
  }

    toggleOptions(pincode_id: any) {
    if (this.showOptionsDivId === pincode_id) {
      this.showOptionsDivId = null;
    } else {
      this.showOptionsDivId = pincode_id;
    }
  }

  ondelete() {
    console.log(this.parameterValue);
    var url = 'Pincode/DeletePincode'
    let param = {
      pincode_id: this.parameterValue
    }
    this.service.getparams(url, param).subscribe((result: any) => {
      if (result.status == false) {
        this.ToastrService.warning(result.message)
        this.GetPincode();
      }
      else {
        this.ToastrService.success(result.message)
        this.GetPincode();
      }
      this.GetPincode();
      this.NgxSpinnerService.hide();
    });
  }

  openModaldelete(parameter: string) {
    this.parameterValue = parameter;
  }
  updateRemainingCharsadd() {
    this.remainingChars = 6 - this.reactiveForm.value.pincode.length;
  }
  preventSpace(event: KeyboardEvent) {
    if (event.key === ' ') {
      event.preventDefault();
    }
  }
}

