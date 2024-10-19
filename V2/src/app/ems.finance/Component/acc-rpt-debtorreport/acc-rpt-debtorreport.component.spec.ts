import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AccRptDebtorreportComponent } from './acc-rpt-debtorreport.component';

describe('AccRptDebtorreportComponent', () => {
  let component: AccRptDebtorreportComponent;
  let fixture: ComponentFixture<AccRptDebtorreportComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [AccRptDebtorreportComponent]
    });
    fixture = TestBed.createComponent(AccRptDebtorreportComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
