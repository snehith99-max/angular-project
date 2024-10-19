import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AccRptCreditorreportComponent } from './acc-rpt-creditorreport.component';

describe('AccRptCreditorreportComponent', () => {
  let component: AccRptCreditorreportComponent;
  let fixture: ComponentFixture<AccRptCreditorreportComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [AccRptCreditorreportComponent]
    });
    fixture = TestBed.createComponent(AccRptCreditorreportComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
