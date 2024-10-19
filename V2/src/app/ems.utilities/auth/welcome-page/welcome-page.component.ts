import { Component } from '@angular/core';
import { SocketService } from '../../services/socket.service';

@Component({
  selector: 'app-welcome-page',
  templateUrl: './welcome-page.component.html',

})
export class WelcomePageComponent {

  Company_list: any[] = [];
  current_domain : any;
  welcome_logo : any;
  constructor(private service: SocketService) { }

  ngOnInit(): void {
    var api = 'CampaignService/GetCompanySummary';
    this.service.get(api).subscribe((result: any) => {
      this.Company_list = result.Company_list;
      this.welcome_logo = result.Company_list[0].welcome_logo;
    });
  }

}
