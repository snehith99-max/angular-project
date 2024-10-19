import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AccRptProfileandlossdetailsComponent } from './acc-rpt-profileandlossdetails.component';

describe('AccRptProfileandlossdetailsComponent', () => {
  let component: AccRptProfileandlossdetailsComponent;
  let fixture: ComponentFixture<AccRptProfileandlossdetailsComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [AccRptProfileandlossdetailsComponent]
    });
    fixture = TestBed.createComponent(AccRptProfileandlossdetailsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
