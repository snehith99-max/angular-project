import { Component } from '@angular/core';
import { Router, UrlTree, UrlSegmentGroup, UrlSegment } from '@angular/router';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent {
  title = 'V2';
  constructor(public router:Router){}

  isInPaymentsPage(): boolean {
    const urlTree: UrlTree = this.router.parseUrl(this.router.url);
    const urlSegmentGroup: UrlSegmentGroup = urlTree.root.children['primary'];
    const urlSegments: UrlSegment[] = urlSegmentGroup.segments;

    // Check if the first segment is 'auth' and the second segment is 'payments'
    return urlSegments.length >= 2 && urlSegments[0].path === 'auth' && urlSegments[1].path === 'payments';
  }
}
