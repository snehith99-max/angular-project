import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AccMstChartofaccountComponent } from './acc-mst-chartofaccount.component';

describe('AccMstChartofaccountComponent', () => {
  let component: AccMstChartofaccountComponent;
  let fixture: ComponentFixture<AccMstChartofaccountComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [AccMstChartofaccountComponent]
    });
    fixture = TestBed.createComponent(AccMstChartofaccountComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
