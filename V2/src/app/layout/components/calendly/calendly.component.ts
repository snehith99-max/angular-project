import { Component, Input } from '@angular/core';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import { NgxSpinnerService } from 'ngx-spinner';

@Component({
  selector: 'app-calendly',
  templateUrl: './calendly.component.html',
  styleUrls: ['./calendly.component.scss']
})
export class CalendlyComponent {

  @Input() scheduling_url: string | undefined;
  
  constructor(
    public socketservice: SocketService,
    private NgxSpinnerService: NgxSpinnerService
  ) {
  }

  ngOnInit() {
    // Embed Calendly widget script
    const script = document.createElement('script');
    script.type = 'text/javascript';
    script.src = 'https://assets.calendly.com/assets/external/widget.js';
    script.async = true;
    // script.onload = this.loadCalendlyWidget.bind(this);
    document.body.appendChild(script);
  }

  getdataurl(){
    console.log(this.scheduling_url)
    return this.scheduling_url
  }
}
