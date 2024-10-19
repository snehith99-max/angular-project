import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AccMstChartofaccountincomeComponent } from './acc-mst-chartofaccountincome.component';

describe('AccMstChartofaccountincomeComponent', () => {
  let component: AccMstChartofaccountincomeComponent;
  let fixture: ComponentFixture<AccMstChartofaccountincomeComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [AccMstChartofaccountincomeComponent]
    });
    fixture = TestBed.createComponent(AccMstChartofaccountincomeComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
