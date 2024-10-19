import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AccRptDebtorreportviewComponent } from './acc-rpt-debtorreportview.component';

describe('AccRptDebtorreportviewComponent', () => {
  let component: AccRptDebtorreportviewComponent;
  let fixture: ComponentFixture<AccRptDebtorreportviewComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [AccRptDebtorreportviewComponent]
    });
    fixture = TestBed.createComponent(AccRptDebtorreportviewComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
