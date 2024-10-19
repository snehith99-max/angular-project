
import { Component } from '@angular/core';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import { NgxSpinnerService } from 'ngx-spinner';
import { SelectionModel } from '@angular/cdk/collections';
import { Table } from 'primeng/table';
import { ToastrService } from 'ngx-toastr';
import { FormControl, FormGroup } from '@angular/forms';
import { Router } from '@angular/router';


export class IMapTaxsegment {
  GetUnassign_list: string[] = [];
  taxsegment_gid: any;
  vendor_gid: any[] = [];
  customer_gid1: any[] = [];
}
@Component({
  selector: 'app-pmr-mst-taxsegment-unassign-vendor',
  templateUrl: './pmr-mst-taxsegment-unassign-vendor.component.html',
})

export class PmrMstTaxsegmentUnassignVendorComponent {

  GetUnassign_list: any[] = [];
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
    public ToastrService: ToastrService,private route: Router) { }

  ngOnInit(): void {
    this.GetTaxSegCount()
    this.GetUnassign()
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
  GetUnassign(){
    var othersegmentapi = 'PmrTaxSegment/GetUnassignSummary';
    $('#GetUnassign_list').DataTable().destroy();
    this.service.get(othersegmentapi).subscribe((apiresponse: any) => {
      this.GetUnassign_list = apiresponse.GetPmrInassignlist;
      setTimeout(() => {
        $('#GetUnassign_list').DataTable();
      }, 1);
      this.taxsegment_gid = apiresponse.GetPmrInassignlist[0].taxsegment_gid;
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
    const numRows = this.GetUnassign_list.length;
    return numSelected === numRows;
  }
  masterToggle1() {
    this.isAllSelected1() ?
      this.selection1.clear() :
      this.GetUnassign_list.forEach((row: IMapTaxsegment) => this.selection1.select(row));
  }



  onSubmit() {
    debugger
    const taxsegment_gid = this.taxsegmentform.value.taxsegment_gid;


    this.pick = this.selection1.selected;
    let vendorgid = this.pick.map(item => item.vendor_gid);


    this.CurObj1.GetUnassign_list = this.pick;
    this.CurObj1.taxsegment_gid = taxsegment_gid;
    this.CurObj1.customer_gid1 = vendorgid;

    let list = this.pick;
    this.CurObj1.GetUnassign_list = list;
    {
      if (this.CurObj1.GetUnassign_list.length !== 0) {        
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
            this.GetUnassign()
          } else {
            window.scrollTo({
              top: 0,
              behavior: 'smooth'
          });
            this.ToastrService.success(result.message);
            this.GetTaxSegCount()
            this.NgxSpinnerService.hide();
            this.selection1.clear();
            this.GetUnassign()
          }


        });

        this.NgxSpinnerService.hide();
        this.selection1.clear();

      } else {
        this.ToastrService.warning("Kindly Select At Least One Record to Move Product!");
      }
    }
  }

  onback(){
    this.route.navigate(['/pmr/PmrMstTaxSegment']);
  }

}
