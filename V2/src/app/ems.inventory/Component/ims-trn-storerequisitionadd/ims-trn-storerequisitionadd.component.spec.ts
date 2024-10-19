import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ImsTrnStorerequisitionaddComponent } from './ims-trn-storerequisitionadd.component';

describe('ImsTrnStorerequisitionaddComponent', () => {
  let component: ImsTrnStorerequisitionaddComponent;
  let fixture: ComponentFixture<ImsTrnStorerequisitionaddComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [ImsTrnStorerequisitionaddComponent]
    });
    fixture = TestBed.createComponent(ImsTrnStorerequisitionaddComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
