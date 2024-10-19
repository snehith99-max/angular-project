import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ImsTrnStorerequisitionComponent } from './ims-trn-storerequisition.component';

describe('ImsTrnStorerequisitionComponent', () => {
  let component: ImsTrnStorerequisitionComponent;
  let fixture: ComponentFixture<ImsTrnStorerequisitionComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [ImsTrnStorerequisitionComponent]
    });
    fixture = TestBed.createComponent(ImsTrnStorerequisitionComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
