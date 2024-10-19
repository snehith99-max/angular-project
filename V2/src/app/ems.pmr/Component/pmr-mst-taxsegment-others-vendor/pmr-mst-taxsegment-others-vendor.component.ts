
import { Component } from '@angular/core';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import { NgxSpinnerService } from 'ngx-spinner';
import { SelectionModel } from '@angular/cdk/collections';
import { Table } from 'primeng/table';
import { ToastrService } from 'ngx-toastr';
import { FormControl, FormGroup } from '@angular/forms';


export class IMapTaxsegment {
  Getothers_list: string[] = [];
  taxsegment_gid: any;
  vendor_gid: any[] = [];
  customer_gid1: any[] = [];
}
@Component({
  selector: 'app-pmr-mst-taxsegment-others-vendor',
  templateUrl: './pmr-mst-taxsegment-others-vendor.component.html',
  styleUrls: ['./pmr-mst-taxsegment-others-vendor.component.scss']
})

export class PmrMstTaxsegmentOthersVendorComponent {

  Getothers_list: any[] = [];
  GetTaxSegmentDropDown_list: any[] = [];
  taxsegment_gid: any;
  pick: Array<any> = [];
  selection1 = new SelectionModel<IMapTaxsegment>(true, []);
  CurObj1: IMapTaxsegment = new IMapTaxsegment();
  customer_gid1: any;
  TaxSegmentVendor_list :any[]=[];
  responsedata:any;

  taxsegmentform!: FormGroup;
  constructor(public service: SocketService,
    private NgxSpinnerService: NgxSpinnerService,
    public ToastrService: ToastrService) { }

  ngOnInit(): void {
    this.GetTaxSegCount()
    this.GetOtherVendor();
    this.taxsegmentform = new FormGroup({
      taxsegment_gid: new FormControl(''),
    })
    
  }
  GetTaxSegCount(){
    var api = 'SmrMstTaxSegment/GetVendorrCount'
    this.service.get(api).subscribe((result: any) => {
      this.responsedata = result;
      this.TaxSegmentVendor_list = this.responsedata.TaxSegmentVendor_list;

    });
  }
  GetOtherVendor(){
    var othersegmentapi = 'PmrTaxSegment/GetOthersSegmentSummary';
    $('#Getothers_list').DataTable().destroy();
    this.service.get(othersegmentapi).subscribe((apiresponse: any) => {
      this.Getothers_list = apiresponse.GetPmrothers_list;
      setTimeout(() => {
        $('#Getothers_list').DataTable();
      }, 1);
      this.taxsegment_gid = apiresponse.GetPmrothers_list[0].taxsegment_gid;
      this.GetTaxSegment(this.taxsegment_gid);
    });
  }
  GetTaxSegment(taxsegment_gid: any) {
    debugger
    let params = { taxsegment_gid: taxsegment_gid };
    var othersegmentapi = 'PmrTaxSegment/GetTaxSegmentDropDown';
    this.service.getparams(othersegmentapi, params).subscribe((apiresponse: any) => {
      this.GetTaxSegmentDropDown_list = apiresponse.GetPmrTaxSegmentDropDown_list;
    });
  }
  isAllSelected1() {
    const numSelected = this.selection1.selected.length;
    const numRows = this.Getothers_list.length;
    return numSelected === numRows;
  }
  masterToggle1() {
    this.isAllSelected1() ?
      this.selection1.clear() :
      this.Getothers_list.forEach((row: IMapTaxsegment) => this.selection1.select(row));
  }



  onSubmit() {
    debugger
    const taxsegment_gid = this.taxsegmentform.value.taxsegment_gid;


    this.pick = this.selection1.selected;
    let vendorgid = this.pick.map(item => item.vendor_gid);


    this.CurObj1.Getothers_list = this.pick;
    this.CurObj1.taxsegment_gid = taxsegment_gid;
    this.CurObj1.customer_gid1 = vendorgid;

    let list = this.pick;
    this.CurObj1.Getothers_list = list;
    {
      if (this.CurObj1.Getothers_list.length !== 0) {        
        this.NgxSpinnerService.show();
        var url1 = 'PmrTaxSegment/PostTaxsegmentMoveOn';
        this.service.post(url1, this.CurObj1).pipe().subscribe((result: any) => {
          if (result.status === false) {
            window.scrollTo({
              top: 0,
              behavior: 'smooth'
          });
            this.NgxSpinnerService.hide();
            this.GetTaxSegCount()
            this.selection1.clear();
            this.GetOtherVendor()

          } else {
            window.scrollTo({
              top: 0,
              behavior: 'smooth'
          });
            this.ToastrService.success(result.message);
            this.GetTaxSegCount()
            this.NgxSpinnerService.hide();
            this.selection1.clear();
            this.GetOtherVendor()
          }


        });

        this.NgxSpinnerService.hide();
        this.selection1.clear();

      } else {
        this.ToastrService.warning("Kindly Select At Least One Record to Move Product!");
      }
    }
  }



}
