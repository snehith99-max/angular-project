import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SmrRptCustomerreportViewComponent } from './smr-rpt-customerreport-view.component';

describe('SmrRptCustomerreportViewComponent', () => {
  let component: SmrRptCustomerreportViewComponent;
  let fixture: ComponentFixture<SmrRptCustomerreportViewComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [SmrRptCustomerreportViewComponent]
    });
    fixture = TestBed.createComponent(SmrRptCustomerreportViewComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
