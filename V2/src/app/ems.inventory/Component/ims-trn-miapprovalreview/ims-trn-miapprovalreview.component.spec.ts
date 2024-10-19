import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ImsTrnMIapprovalreviewComponent } from './ims-trn-miapprovalreview.component';

describe('ImsTrnMIapprovalreviewComponent', () => {
  let component: ImsTrnMIapprovalreviewComponent;
  let fixture: ComponentFixture<ImsTrnMIapprovalreviewComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [ImsTrnMIapprovalreviewComponent]
    });
    fixture = TestBed.createComponent(ImsTrnMIapprovalreviewComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
