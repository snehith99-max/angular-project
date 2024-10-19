import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ImsRptHighcostComponent } from './ims-rpt-highcost.component';

describe('ImsRptHighcostComponent', () => {
  let component: ImsRptHighcostComponent;
  let fixture: ComponentFixture<ImsRptHighcostComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [ImsRptHighcostComponent]
    });
    fixture = TestBed.createComponent(ImsRptHighcostComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
