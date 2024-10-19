import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AccMstChartofaccountassetviewComponent } from './acc-mst-chartofaccountassetview.component';

describe('AccMstChartofaccountassetviewComponent', () => {
  let component: AccMstChartofaccountassetviewComponent;
  let fixture: ComponentFixture<AccMstChartofaccountassetviewComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [AccMstChartofaccountassetviewComponent]
    });
    fixture = TestBed.createComponent(AccMstChartofaccountassetviewComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
