import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AccRptOpenbalancereportComponent } from './acc-rpt-openbalancereport.component';

describe('AccRptOpenbalancereportComponent', () => {
  let component: AccRptOpenbalancereportComponent;
  let fixture: ComponentFixture<AccRptOpenbalancereportComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [AccRptOpenbalancereportComponent]
    });
    fixture = TestBed.createComponent(AccRptOpenbalancereportComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
