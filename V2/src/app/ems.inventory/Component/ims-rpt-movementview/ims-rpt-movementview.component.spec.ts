import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ImsRptMovementviewComponent } from './ims-rpt-movementview.component';

describe('ImsRptMovementviewComponent', () => {
  let component: ImsRptMovementviewComponent;
  let fixture: ComponentFixture<ImsRptMovementviewComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [ImsRptMovementviewComponent]
    });
    fixture = TestBed.createComponent(ImsRptMovementviewComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
