import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AccMstChartofaccountliabilityviewComponent } from './acc-mst-chartofaccountliabilityview.component';

describe('AccMstChartofaccountliabilityviewComponent', () => {
  let component: AccMstChartofaccountliabilityviewComponent;
  let fixture: ComponentFixture<AccMstChartofaccountliabilityviewComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [AccMstChartofaccountliabilityviewComponent]
    });
    fixture = TestBed.createComponent(AccMstChartofaccountliabilityviewComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
