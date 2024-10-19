import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ImsRptStockagereportComponent } from './ims-rpt-stockagereport.component';

describe('ImsRptStockagereportComponent', () => {
  let component: ImsRptStockagereportComponent;
  let fixture: ComponentFixture<ImsRptStockagereportComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [ImsRptStockagereportComponent]
    });
    fixture = TestBed.createComponent(ImsRptStockagereportComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
