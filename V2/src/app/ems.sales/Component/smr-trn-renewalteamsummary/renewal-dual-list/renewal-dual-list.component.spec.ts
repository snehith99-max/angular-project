import { ComponentFixture, TestBed } from '@angular/core/testing';

import { RenewalDualListComponent } from './renewal-dual-list.component';

describe('RenewalDualListComponent', () => {
  let component: RenewalDualListComponent;
  let fixture: ComponentFixture<RenewalDualListComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [RenewalDualListComponent]
    });
    fixture = TestBed.createComponent(RenewalDualListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
