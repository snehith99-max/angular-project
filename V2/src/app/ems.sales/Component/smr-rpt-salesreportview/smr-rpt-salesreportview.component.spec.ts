import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SmrRptSalesreportviewComponent } from './smr-rpt-salesreportview.component';

describe('SmrRptSalesreportviewComponent', () => {
  let component: SmrRptSalesreportviewComponent;
  let fixture: ComponentFixture<SmrRptSalesreportviewComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [SmrRptSalesreportviewComponent]
    });
    fixture = TestBed.createComponent(SmrRptSalesreportviewComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
