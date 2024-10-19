import { Component } from '@angular/core';
import { FormBuilder, FormControl, FormGroup } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { AngularEditorConfig } from '@kolkov/angular-editor';
import { AES, enc } from 'crypto-js';
import { NgxSpinnerService } from 'ngx-spinner';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';

@Component({
  selector: 'app-smr-trn-salesorderview-new',
  templateUrl: './smr-trn-salesorderview-new.component.html',
  styleUrls: ['./smr-trn-salesorderview-new.component.scss']
})
export class SmrTrnSalesorderviewNewComponent {
  config: AngularEditorConfig = {
    editable: false,
    spellcheck: false,
    height: '40rem',
    minHeight: '5rem',
    width: '750px',
    placeholder: 'Enter text here...',
    translate: 'no',
    defaultParagraphSeparator: 'p',
    defaultFontName: 'Arial',
  };
  orderviewform: FormGroup | any;
  Viewsalesordersummary_list: any[] = [];
  Viewsalesorderdetail_list: any[] = [];
  gid_list: any[] = [];
  GetTaxSegmentList : any[]=[];
  lead2campaign_gid: any;
  customer: any;
  responsedata: any;
  leadbank_gid: any;
  lspage: any;
  customer_gid:any;

  constructor(private formBuilder: FormBuilder,
     public NgxSpinnerService: NgxSpinnerService,
      private route: Router, 
      private router: ActivatedRoute, public service: SocketService) {
    this.orderviewform = new FormGroup({
      termsandconditions: new FormControl(''),
      net_amount: new FormControl(''),
      tax_name: new FormControl(''),
      total_amountwithtax: new FormControl(''),
      addon_charges: new FormControl(''),
      additional_discount: new FormControl(''),
      freight_charges: new FormControl(''),
      buyback_charges: new FormControl(''),
      packing_charges: new FormControl(''),
      insurance_charges: new FormControl(''),
      roundoff: new FormControl(''),
      grandtotal: new FormControl('')
    })
  }

  ngOnInit(): void {
    const salesorder_gid = this.router.snapshot.paramMap.get('salesorder_gid');
    const leadbank_gid = this.router.snapshot.paramMap.get('leadbank_gid');
    const lspage = this.router.snapshot.paramMap.get('lspage');
    const customer_gid = this.router.snapshot.paramMap.get('customer_gid');
    this.customer_gid = customer_gid

    this.customer = salesorder_gid;
    this.leadbank_gid = leadbank_gid;
    this.lspage = lspage;

    const secretKey = 'storyboarderp';
    const deencryptedParam = AES.decrypt(this.customer, secretKey).toString(enc.Utf8);
    const deencryptedParam1 = AES.decrypt(this.leadbank_gid, secretKey).toString(enc.Utf8);
    const deencryptedParam2 = AES.decrypt(this.lspage, secretKey).toString(enc.Utf8);
    const deencryptedParam3 = AES.decrypt(this.customer_gid, secretKey).toString(enc.Utf8);

    this.lspage = deencryptedParam2;

    console.log(deencryptedParam)
    console.log(deencryptedParam1);

    this.GetViewsalesorderSummary(deencryptedParam);
    this.GetViewsalesorderdetails(deencryptedParam,deencryptedParam3);
    this.GetGiddetails(deencryptedParam);
  }

  // customer Details
  GetViewsalesorderSummary(salesorder_gid: any) {
    var url = 'SmrTrnSalesorder/GetViewsalesorderSummary'
    let param = {
      salesorder_gid: salesorder_gid
    }
    this.service.getparams(url, param).subscribe((result: any) => {
      this.responsedata = result;
      this.Viewsalesordersummary_list = result.postsalesorder_list;
      this.orderviewform.get("termsandconditions")?.setValue(this.Viewsalesordersummary_list[0].termsandconditions);
      this.orderviewform.get("net_amount")?.setValue(this.Viewsalesordersummary_list[0].total_amount);
      this.orderviewform.get("tax_name")?.setValue(this.Viewsalesordersummary_list[0].tax_name);
      this.orderviewform.get("total_amountwithtax")?.setValue(this.Viewsalesordersummary_list[0].total_price);
      this.orderviewform.get("addon_charges")?.setValue(this.Viewsalesordersummary_list[0].addon_charge);
      this.orderviewform.get("additional_discount")?.setValue(this.Viewsalesordersummary_list[0].additional_discount);
      this.orderviewform.get("freight_charges")?.setValue(this.Viewsalesordersummary_list[0].freight_charges);
      this.orderviewform.get("buyback_charges")?.setValue(this.Viewsalesordersummary_list[0].buyback_charges);
      this.orderviewform.get("packing_charges")?.setValue(this.Viewsalesordersummary_list[0].packing_charges);
      this.orderviewform.get("insurance_charges")?.setValue(this.Viewsalesordersummary_list[0].insurance_charges);
      this.orderviewform.get("roundoff")?.setValue(this.Viewsalesordersummary_list[0].roundoff);
    });
  }

  // Product Details
  GetViewsalesorderdetails(salesorder_gid: any,customer_gid:any) {
    var url = 'SmrTrnSalesorder/GetViewsalesorderdetails'
    this.NgxSpinnerService.show()
    let param = {
      salesorder_gid: salesorder_gid,
      customer_gid:customer_gid
    }
    this.service.getparams(url, param).subscribe((result: any) => {
      this.responsedata = result;
      this.Viewsalesorderdetail_list = result.postsalesorderdetails_list;
      this.NgxSpinnerService.hide()
    });
  }


  //GetGidDetails
  GetGiddetails(leadbank_gid: any) {
    debugger
    var url = 'Leadbank360/GetGidDetails'
    let param = {
      leadbank_gid: leadbank_gid
    }
    this.service.getparams(url, param).subscribe((result: any) => {
      this.responsedata = result;
      this.gid_list = this.responsedata.gid_list;

      this.leadbank_gid = this.gid_list[0].leadbank_gid;
      this.lead2campaign_gid = this.gid_list[0].lead2campaign_gid;
      setTimeout(() => {
        $('#leadorderdetails_list').DataTable();
      }, 1);
    });

  }


  // for tax segment
  fetchProductSummaryAndTax(product_gid: any,salesorder_gid:any,customer_gid:any) {
    debugger
        let param = {
        product_gid: product_gid,
        salesorder_gid: salesorder_gid,
        customer_gid:customer_gid
      };
  
      var api = 'SmrTrnSalesorder/GetViewsalesorderdetails';
      this.service.getparams(api, param).subscribe((result: any) => {
        this.responsedata = result;
        this.GetTaxSegmentList = result.GetTaxSegmentListorder;
  
        // Handle tax segments for the current product
        this.handleTaxSegments(product_gid, this.GetTaxSegmentList);
  
       
      }, (error) => {
        console.error('Error fetching tax details:', error);
        
      });
    
  }
  
  // Method to handle tax segments for the current product
  handleTaxSegments(product_gid: string ,taxSegments: any[]) {
    // Find tax segments for the current product_gid
    const productTaxSegments = taxSegments.filter((taxSegment: { product_gid: string; }) => taxSegment.product_gid === product_gid);
  
    if (productTaxSegments.length > 0) {
      // Assign tax segments to the current product
      this.Viewsalesorderdetail_list.forEach((product: { product_gid: string; taxSegments: any[]; }) => {
        if (product.product_gid === product_gid) {
          product.taxSegments = productTaxSegments;
        }
      });
    } else {
      // No tax segments found for the current product
      console.warn('No tax segments found for product_gid:', product_gid);
    }
  }
  
  // Inside your component class
  checkDuplicateTaxSegment(taxSegments: any[], currentIndex: number): boolean {
    // Extract the taxsegment_gid of the current tax segment
    const currentTaxSegmentId = taxSegments[currentIndex].taxsegment_gid;
  
    // Check if the current tax segment exists before the current index in the array
    for (let i = 0; i < currentIndex; i++) {
        if (taxSegments[i].taxsegment_gid === currentTaxSegmentId) {
            // Duplicate found
            return true;
        }
    }
  
    // No duplicates found
    return false;
  }
  // Inside your component class
  removeDuplicateTaxSegments(taxSegments: any[]): any[] {
    const uniqueTaxSegmentsMap = new Map<string, any>();
    taxSegments.forEach(taxSegment => {
        uniqueTaxSegmentsMap.set(taxSegment.tax_name, taxSegment);
    });
    // Convert the Map back to an array
    return Array.from(uniqueTaxSegmentsMap.values());
  }

  // back routing
  onback() {
    window.history.back();
  } 
}
