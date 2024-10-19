import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ImsTrnMIapprovalComponent } from './ims-trn-miapproval.component';

describe('ImsTrnMIapprovalComponent', () => {
  let component: ImsTrnMIapprovalComponent;
  let fixture: ComponentFixture<ImsTrnMIapprovalComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [ImsTrnMIapprovalComponent]
    });
    fixture = TestBed.createComponent(ImsTrnMIapprovalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
