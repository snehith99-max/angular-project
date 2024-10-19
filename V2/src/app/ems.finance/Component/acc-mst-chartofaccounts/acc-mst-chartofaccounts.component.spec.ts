import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AccMstChartofaccountsComponent } from './acc-mst-chartofaccounts.component';

describe('AccMstChartofaccountsComponent', () => {
  let component: AccMstChartofaccountsComponent;
  let fixture: ComponentFixture<AccMstChartofaccountsComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [AccMstChartofaccountsComponent]
    });
    fixture = TestBed.createComponent(AccMstChartofaccountsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
