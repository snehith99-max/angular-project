import { Component } from '@angular/core';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import { NgxSpinnerService } from 'ngx-spinner';
import { ToastrService } from 'ngx-toastr';
import { environment } from 'src/environments/environment';
import { ActivatedRoute, Router } from '@angular/router';
import { AES, enc } from 'crypto-js';
import { FormControl, FormGroup } from '@angular/forms';
import { ChangeDetectorRef } from '@angular/core';
import flatpickr from 'flatpickr';
import { Options } from 'flatpickr/dist/types/options';

@Component({
  selector: 'app-ims-trn-mrpriceassign-view',
  templateUrl: './ims-trn-mrpriceassign-view.component.html',
  styleUrls: ['./ims-trn-mrpriceassign-view.component.scss']
})
export class ImsTrnMrpriceassignViewComponent {

  IDPRForm!: FormGroup;
  materialrequisition_gid: any;
  materialrequisitiongid: any;
  Mdlunit_price: any;
  Mdlprovisional: any;
  GetIDPREstimationDetails_list: any[] = [];
  GetIDPREstimationProductDetails_list: any[] = [];
  GetIDPRProductDetailsCheckPrice_list: any[] = [];

  constructor(private serivce: SocketService,
    private NgxSpinnerService: NgxSpinnerService,
    private ToastrService: ToastrService,
    private route: ActivatedRoute,
    private router: Router,
    private cdr: ChangeDetectorRef
  ) { }

  ngOnInit(): void {
    debugger
    const options: Options = {
      dateFormat: 'd-m-Y',
    };
    flatpickr('.date-picker', options);
    const key = environment.secretKey;
    this.materialrequisitiongid = this.route.snapshot.paramMap.get('materialrequisition_gid');
    this.materialrequisition_gid = AES.decrypt(this.materialrequisitiongid, key).toString(enc.Utf8);
    this.GetIDPREstimationDetails(this.materialrequisition_gid);
    this.GetIDPREstimationDetailsProduct(this.materialrequisition_gid);

    this.IDPRForm = new FormGroup({
      branch_name: new FormControl(''),
      mi_ref_no: new FormControl(''),
      mi_date: new FormControl(this.getCurrentDate()),
      department_name: new FormControl(''),
      delivery_address: new FormControl(''),
      raise_by: new FormControl(''),
      costcenter_name: new FormControl(''),
      mi_remarks: new FormControl(''),
      approval_remarks: new FormControl(''),
      costcenter_amount: new FormControl(''),
    });
  }
  getCurrentDate(): string {
    const today = new Date();
    const dd = String(today.getDate()).padStart(2, '0');
    const mm = String(today.getMonth() + 1).padStart(2, '0');
    const yyyy = today.getFullYear();
    return dd + '-' + mm + '-' + yyyy;
  }
  GetIDPREstimationDetails(materialrequisition_gid: any) {
    debugger
    let param = { materialrequisition_gid: materialrequisition_gid };
    this.NgxSpinnerService.show();

    const apidetails = 'IndentPriceEstimation/GetIDTPREstimateDetails';
    this.serivce.getparams(apidetails, param).subscribe((result: any) => {
      this.GetIDPREstimationDetails_list = result.GetIDPREstimationDetails_list;
      this.IDPRForm.get('branch_name')?.setValue(this.GetIDPREstimationDetails_list[0].branch_name);
      this.IDPRForm.get('department_name')?.setValue(this.GetIDPREstimationDetails_list[0].department_name);
      this.IDPRForm.get('costcenter_name')?.setValue(this.GetIDPREstimationDetails_list[0].costcenter_name);
      this.IDPRForm.get('costcenter_amount')?.setValue(this.GetIDPREstimationDetails_list[0].costcenter_amount);
      this.IDPRForm.get('mi_ref_no')?.setValue(this.GetIDPREstimationDetails_list[0].materialrequisition_gid);
      this.IDPRForm.get('mi_date')?.setValue(this.GetIDPREstimationDetails_list[0].materialrequisition_date);
      this.IDPRForm.get('raise_by')?.setValue(this.GetIDPREstimationDetails_list[0].user_firstname);
      this.IDPRForm.get('mi_ref')?.setValue(this.GetIDPREstimationDetails_list[0].materialrequisition_reference);
      this.IDPRForm.get('mi_remarks')?.setValue(this.GetIDPREstimationDetails_list[0].materialrequisition_remarks);
      this.IDPRForm.get('approval_remarks')?.setValue(this.GetIDPREstimationDetails_list[0].approver_remarks);

      this.NgxSpinnerService.hide();
    });
  }
  GetIDPREstimationDetailsProduct(materialrequisition_gid: any) {
    let param = { materialrequisition_gid: materialrequisition_gid };
    this.NgxSpinnerService.show();

    const productapi = 'IndentPriceEstimation/GetIDTPREstimateProductDetails';
    this.serivce.getparams(productapi, param).subscribe((result: any) => {
      this.GetIDPREstimationProductDetails_list = result.GetIDPREstimationProductDetails_list;
      this.NgxSpinnerService.hide();
    });
  }
  checkPreviousPrices(product_gid: any) {
    let param = { product_gid: product_gid }
    this.NgxSpinnerService.show();

    const selectapi = 'IndentPriceEstimation/GetIDPRProductDetailsCheckPrice';
    this.serivce.getparams(selectapi, param).subscribe((result: any) => {
      this.GetIDPRProductDetailsCheckPrice_list = result.GetIDPRProductDetailsCheckPrice_list;
      this.NgxSpinnerService.hide();
    });
  }
  Onclickprice(data: any) {
    debugger
    const { product_gid, price } = data
    const product_price = this.GetIDPRProductDetailsCheckPrice_list.find((item:
      { product_gid: any; price: any }) => item.product_gid === product_gid);

    if (product_price.product_gid === product_gid) {
      data.unit_price = price;
      const existingProduct = this.GetIDPREstimationProductDetails_list.find(
        (item) => item.product_gid === product_gid
      );
      existingProduct.unit_price = data.unit_price;
    }
  }
  Generate() {
    debugger
    let param = {
      GetIDPREstimationProductDetails_list: this.GetIDPREstimationProductDetails_list,
      materialrequisition_gid: this.materialrequisition_gid
    }
    const api = 'IndentPriceEstimation/UpdateProductPriceGenerate';
    this.serivce.post(api, param).subscribe((result: any) => {
      this.Mdlprovisional = result.provisional
    });
  }
  onSubmit() {
    let param = {
      materialrequisition_gid: this.materialrequisition_gid,
      provisional_amount: this.Mdlprovisional
    }
    this.NgxSpinnerService.show();

    const postapi = 'IndentPriceEstimation/UpdateProvisionalAmount';
    this.serivce.getparams(postapi, param).subscribe((result: any) => {
      if (result.status == true) {
        this.ToastrService.success(result.message);
        this.router.navigate(['/ims/ImsTrnIndentPriceEstimation']);
      }
      else {
        this.ToastrService.warning('Error while updating provisional amount');
      }
    });
  }
}
