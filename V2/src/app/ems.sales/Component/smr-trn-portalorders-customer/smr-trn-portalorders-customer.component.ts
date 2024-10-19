import { Component } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { AES } from 'crypto-js';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';

@Component({
  selector: 'app-smr-trn-portalorders-customer',
  templateUrl: './smr-trn-portalorders-customer.component.html',
  styleUrls: ['./smr-trn-portalorders-customer.component.scss']
})
export class SmrTrnPortalordersCustomerComponent {

  showOptionsDivId: any;
  salesorder_list: any[] = [];
  CustomerPortalSalesOrder_list: any[] = [];

  constructor(public service: SocketService,
     public route: ActivatedRoute,
     private router: Router) { }

  ngOnInit(): void {

    var summaryapi = 'CustomerPortalOrders/GetCustomerPortalSalesordersummary';
    this.service.get(summaryapi).subscribe((result: any) => {
      this.CustomerPortalSalesOrder_list = result.CustomerPortalSalesOrder_list;
      setTimeout(() => {
        $('#CustomerPortalSalesOrder_list').DataTable();
      }, 1);
    });

  }
  toggleOptions(salesorder_gid: any) {
    if (this.showOptionsDivId === salesorder_gid) {
      this.showOptionsDivId = null;
    } else {
      this.showOptionsDivId = salesorder_gid;
    }
  }
  onview(salesorder_gid: any, customer_gid: any) {
    const secretKey = 'storyboarderp';
    const salesordergid = (salesorder_gid);
    const customergid = (customer_gid);    
    const salesordergid1 = AES.encrypt(salesordergid, secretKey).toString();
    const customergid1 = AES.encrypt(customergid, secretKey).toString();
    this.router.navigate(['/smr/SmrTrnPortalOrderApproval', salesordergid1, customergid1]);
  }
}
