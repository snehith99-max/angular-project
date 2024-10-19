import { query } from '@angular/animations';
import { Component } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { SocketService } from '../services/socket.service';

@Component({
  selector: 'app-payments',
  templateUrl: './payments.component.html',
  styleUrls: ['./payments.component.scss']
})
export class PaymentsComponent {

  constructor(private route: ActivatedRoute, private service: SocketService) {
    const queryParams = this.route.snapshot.queryParamMap;
    const paramValue1 = queryParams.get('v1');
    const paramValue2 = queryParams.get('v2');
    var params = {
      order_gid: paramValue1,
      c_code: paramValue2
    }
    var api = 'Login/updateOrderPayment';
    this.service.post(api, params).subscribe((result: any) => {
    });

  }

}
