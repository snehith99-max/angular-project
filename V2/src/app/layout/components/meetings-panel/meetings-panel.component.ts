import { Component } from '@angular/core';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import { NgxSpinnerService } from 'ngx-spinner';


@Component({
  selector: 'layout-meetings-panel',
  templateUrl: './meetings-panel.component.html',
  styleUrls: ['./meetings-panel.component.scss'],
  
})
export class MeetingsPanelComponent {

  meeting_data: any[] = [] 
  showDefault: boolean = false

  constructor(
    public socketservice: SocketService,
    private NgxSpinnerService: NgxSpinnerService
  ) {
  }

  ngOnInit(){
    this.NgxSpinnerService.show()
    var url = 'Features/calendlyMeeting'
    this.socketservice.get(url).subscribe((result: any) => {
      if(result.status == true){
        this.meeting_data = result.calendlyMeetingList        
      }
      
      this.NgxSpinnerService.hide()
      if(this.meeting_data.length == 0)
        this.showDefault = true
      console.log(this.showDefault)
    });
  }
  ngOnDestroy(): void{

  }

  openLink(url: any) {
    // Perform your logic here, such as navigating to a different route or opening a URL in a new tab
    // Example: Open a URL in a new tab
    console.log(url)
    if(url!="" && url != undefined && url!= null)
      window.open(url, '_blank');

  }
}
