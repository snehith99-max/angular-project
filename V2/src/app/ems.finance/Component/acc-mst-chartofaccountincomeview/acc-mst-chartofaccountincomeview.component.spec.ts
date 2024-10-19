import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AccMstChartofaccountincomeviewComponent } from './acc-mst-chartofaccountincomeview.component';

describe('AccMstChartofaccountincomeviewComponent', () => {
  let component: AccMstChartofaccountincomeviewComponent;
  let fixture: ComponentFixture<AccMstChartofaccountincomeviewComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [AccMstChartofaccountincomeviewComponent]
    });
    fixture = TestBed.createComponent(AccMstChartofaccountincomeviewComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
