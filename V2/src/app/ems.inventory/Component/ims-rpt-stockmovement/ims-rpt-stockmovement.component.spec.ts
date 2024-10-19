import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ImsRptStockmovementComponent } from './ims-rpt-stockmovement.component';

describe('ImsRptStockmovementComponent', () => {
  let component: ImsRptStockmovementComponent;
  let fixture: ComponentFixture<ImsRptStockmovementComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [ImsRptStockmovementComponent]
    });
    fixture = TestBed.createComponent(ImsRptStockmovementComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
