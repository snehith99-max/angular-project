import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AccMstChartofaccountliabilityComponent } from './acc-mst-chartofaccountliability.component';

describe('AccMstChartofaccountliabilityComponent', () => {
  let component: AccMstChartofaccountliabilityComponent;
  let fixture: ComponentFixture<AccMstChartofaccountliabilityComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [AccMstChartofaccountliabilityComponent]
    });
    fixture = TestBed.createComponent(AccMstChartofaccountliabilityComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
