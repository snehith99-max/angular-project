import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ImsTrnRaisematerialindentComponent } from './ims-trn-raisematerialindent.component';

describe('ImsTrnRaisematerialindentComponent', () => {
  let component: ImsTrnRaisematerialindentComponent;
  let fixture: ComponentFixture<ImsTrnRaisematerialindentComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [ImsTrnRaisematerialindentComponent]
    });
    fixture = TestBed.createComponent(ImsTrnRaisematerialindentComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
