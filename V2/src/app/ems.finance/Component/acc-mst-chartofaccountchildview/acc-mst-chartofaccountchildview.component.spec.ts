import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AccMstChartofaccountchildviewComponent } from './acc-mst-chartofaccountchildview.component';

describe('AccMstChartofaccountchildviewComponent', () => {
  let component: AccMstChartofaccountchildviewComponent;
  let fixture: ComponentFixture<AccMstChartofaccountchildviewComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [AccMstChartofaccountchildviewComponent]
    });
    fixture = TestBed.createComponent(AccMstChartofaccountchildviewComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
