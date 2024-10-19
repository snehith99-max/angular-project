import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ImsTrnRolsettingsComponent } from './ims-trn-rolsettings.component';

describe('ImsTrnRolsettingsComponent', () => {
  let component: ImsTrnRolsettingsComponent;
  let fixture: ComponentFixture<ImsTrnRolsettingsComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [ImsTrnRolsettingsComponent]
    });
    fixture = TestBed.createComponent(ImsTrnRolsettingsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
