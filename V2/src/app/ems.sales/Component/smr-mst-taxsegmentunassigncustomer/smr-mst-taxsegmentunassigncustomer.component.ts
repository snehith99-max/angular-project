import { Component } from '@angular/core';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import { NgxSpinnerService } from 'ngx-spinner';
import { SelectionModel } from '@angular/cdk/collections';
import { Table } from 'primeng/table';
import { ToastrService } from 'ngx-toastr';
import { FormControl, FormGroup } from '@angular/forms';
import { Router, ActivatedRoute, Route } from '@angular/router';
import { AES, enc } from 'crypto-js';
import { DomSanitizer } from '@angular/platform-browser';
import { ScrollerModule } from 'primeng/scroller';

export class IunAssign {
  unassigncustomerchecklist: any[] = [];
  mailmanagement_gid: string = "";
  template_gid: string = "";
  leadbank_gid: string = "";
  customer_gid:any;
  GetUnassignsummary_list:any;
  taxsegment_gid: any;

}

export class IMapTaxsegment{
  GetUnassignsummary_list1  : string[] =[];
  customer_gid: any ;
  taxsegment_gid: any;
  customer_gid1: any[]=[];
}
export interface Customer {
  
 
  names?: any | null;
  customer_address?: any | null;
  customer_city?: any | null;
  customer_state?: any | null;
  direction?: any | null;
  source_name?: any | null;
  customer_country?: any | null;

}

@Component({
  selector: 'app-smr-mst-taxsegmentunassigncustomer',
  templateUrl: './smr-mst-taxsegmentunassigncustomer.component.html',
  styleUrls: ['./smr-mst-taxsegmentunassigncustomer.component.scss']
})
export class SmrMstTaxsegmentunassigncustomerComponent {
  taxsegmentform!: FormGroup;
  GetUnassignsummary_list1: any[]=[];
  GetTaxSegmentDropDown_list: any[]=[];
  taxsegment_gid: any;
  customer_gid: any;
  customer!: Customer;
  selectedCustomer: string[] = [];
  customer_gid1: any;
  mdltaxsegment:any;
  TaxSegmentCustomer_list:any[]=[];
  responsedata:any;
  deencryptedParam:any;
  pick: Array<any> = [];
  selection1 = new SelectionModel<IMapTaxsegment>(true, []);
  CurObj1: IMapTaxsegment = new IMapTaxsegment();
  CurObj: IunAssign = new IunAssign();
     selection = new SelectionModel<IunAssign>(true, []);
  constructor (public service: SocketService,
    private NgxSpinnerService: NgxSpinnerService,
    public ToastrService: ToastrService,
    private route: Router, private router: ActivatedRoute,private sanitizer: DomSanitizer
  ){}
  
  ngOnInit() : void {
    debugger
    const taxsegment_gid = this.router.snapshot.paramMap.get('taxsegment_gid1');
    this.taxsegment_gid = taxsegment_gid;
    const secretKey = 'storyboard';
    this.deencryptedParam = AES.decrypt(this.taxsegment_gid, secretKey).toString(enc.Utf8);

  this.taxsegmentform =new FormGroup({
      taxsegment_gid: new FormControl(''),
})
// this.NgxSpinnerService.show();
    var othersegmentapi = 'SmrMstTaxSegment/GetCustomerUnassignSummary';
    this.NgxSpinnerService.show()
    $('#GetUnassignsummary_list1').DataTable().destroy();
    this.service.get(othersegmentapi).subscribe((apiresponse: any) =>{


      if(apiresponse.contact != null){
        $('#GetUnassignsummary_list1').DataTable().destroy();
        this.GetUnassignsummary_list1 = apiresponse.GetUnassignsummary_list;
        this.NgxSpinnerService.hide();
        setTimeout(()=>{
          $('#GetUnassignsummary_list1').DataTable();
        }, 1);
      }
      else{
        this.GetUnassignsummary_list1 = apiresponse.GetUnassignsummary_list;
        setTimeout(()=>{
          $('#GetUnassignsummary_list1').DataTable();
        }, 1);
        this.NgxSpinnerService.hide();
        $('#GetUnassignsummary_list1').DataTable().destroy()
      }
      this.taxsegment_gid = apiresponse.GetUnassignsummary_list[0].taxsegment_gid;       
    });   
    this.totalcount()
  }
  totalcount(){
    var api = 'SmrMstTaxSegment/GetCustomerCount'
    this.service.get(api).subscribe((result: any) => {
      this.responsedata = result;
      this.TaxSegmentCustomer_list = this.responsedata.TaxSegmentCustomer_list;

    });
  }

 

  isAllSelected() {
    const numSelected = this.selection.selected.length;
    const numRows = this.GetUnassignsummary_list1.length;
    return numSelected === numRows;
  }
  masterToggle() {
    this.isAllSelected() ?
      this.selection.clear() :
      this.GetUnassignsummary_list1.forEach((row: IunAssign) => this.selection.select(row));
  }
  assign(){
    debugger;
    this.CurObj.taxsegment_gid = this.deencryptedParam
      this.pick = this.selection.selected  
      this.CurObj.GetUnassignsummary_list = this.pick
      this.CurObj.customer_gid=this.customer_gid
        if (this.CurObj.GetUnassignsummary_list.length === 0) {
        this.ToastrService.warning("Select atleast one customer");
        return;
      } 
  
      debugger
      this.NgxSpinnerService.show();
        var url = 'SmrMstTaxSegment/PostTaxsegmentMoveOn';  
        this.service.post(url, this.CurObj).subscribe((result: any) => {
          if (result.status === false) {
            this.ToastrService.warning(result.message);
            
          } else {
            this.ToastrService.success(result.message);
            this.route.navigate(['/smr/SmrMstTaxsegment'])
            this.NgxSpinnerService.hide();

          }
        });     
      this.selection.clear();
  }


}
