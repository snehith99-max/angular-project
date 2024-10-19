
import { Component, ElementRef, HostListener, Input } from '@angular/core';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import { NgxSpinnerService } from 'ngx-spinner';
import { Observable, interval, Subject } from 'rxjs';
import { takeWhile, map, takeUntil, catchError } from 'rxjs/operators';

@Component({
  selector: 'layout-footer',
  templateUrl: './footer.component.html',
  styleUrls: ['./footer.component.scss']
})
export class FooterComponent {
  calenderopen: boolean = true;
  iscalendlyactive: boolean = false;
  scheduling_url: string | undefined
  private destroy$ = new Subject<void>();


  constructor(
    public socketservice: SocketService,
    private NgxSpinnerService: NgxSpinnerService,
    private _eref: ElementRef
  ) {
    this.waitForToken().subscribe(() => {
      this.calendly()

    });

  }

  ngOnInit(): void {
    
  }
  notesopen: boolean = true;
  @Input() collapsed = false;
  @Input() screenWidth = 0;

  getFooterClass(): string {
    let styleClass = '';
    if (this.collapsed && this.screenWidth > 768) {
      styleClass = 'footer-trimmed';
    } else if (this.collapsed && this.screenWidth <= 768 && this.screenWidth > 0) {
      styleClass = 'footer-md-screen';
    }
    return styleClass;
  }

  notesOpenfunction() {
    this.notesopen = !this.notesopen
  }

  @HostListener('document:click', ['$event'])
  clickout(event: Event) {
    if (!this._eref.nativeElement.contains(event.target)) {
      this.notesopen = true;
      this.calenderopen = true;
    }
  }

  calenderOpenfunction() {
    this.calenderopen = !this.calenderopen
  }

  calendly() {
    this.NgxSpinnerService.show()
    var url = 'Features/calendlyCheckIfActive';
    this.socketservice.get(url).subscribe((result: any) => {
      if (result.status == true) {
        this.NgxSpinnerService.hide()
        this.iscalendlyactive = true
        var url = 'Features/calendlyUserDetails';
        this.socketservice.get(url).subscribe((result: any) => {
          if(result.status == true){
            this.scheduling_url = result.scheduling_url
            console.log(this.scheduling_url)
            if(this.scheduling_url == null || this.scheduling_url == "")
              this.iscalendlyactive = false
          }
        });

        }else {
          this.NgxSpinnerService.hide()
        }
      });
  }

  waitForToken(): Observable<boolean> {
    return interval(2000) // internal every 2 seconds  
      .pipe(
        takeUntil(this.destroy$), // Cleanup when the component is destroyed
        map(() => {
          const token = localStorage.getItem('token');
          return token !== null && token !== '';
        }),
        takeWhile((tokenAvailable) => !tokenAvailable, true),
        catchError((error) => {
          console.error('Error while polling for token:', error);
          return [];
        })
      );
  }

  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }
}
