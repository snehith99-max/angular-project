import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AccMstChartofaccountassetComponent } from './acc-mst-chartofaccountasset.component';

describe('AccMstChartofaccountassetComponent', () => {
  let component: AccMstChartofaccountassetComponent;
  let fixture: ComponentFixture<AccMstChartofaccountassetComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [AccMstChartofaccountassetComponent]
    });
    fixture = TestBed.createComponent(AccMstChartofaccountassetComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
