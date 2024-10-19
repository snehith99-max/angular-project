import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AccRptCreditordetailedreportComponent } from './acc-rpt-creditordetailedreport.component';

describe('AccRptCreditordetailedreportComponent', () => {
  let component: AccRptCreditordetailedreportComponent;
  let fixture: ComponentFixture<AccRptCreditordetailedreportComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [AccRptCreditordetailedreportComponent]
    });
    fixture = TestBed.createComponent(AccRptCreditordetailedreportComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
